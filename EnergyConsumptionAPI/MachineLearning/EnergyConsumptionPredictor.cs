using EnergyConsumptionAPI.Entities;
using EnergyConsumptionAPI.MachineLearning.Models;
using EnergyConsumptionAPI.Repository;
using Microsoft.Data.SqlClient;
using Microsoft.ML;
using Microsoft.ML.AutoML;
using Microsoft.ML.Data;
using Newtonsoft.Json;

namespace EnergyConsumptionAPI.MachineLearning
{
    public class EnergyConsumptionPredictor
    {
        private IConfiguration _configuration;
        private MLContext _mLContext;
        private IEnergyConsumptionForecastRepository _energyConsumptionForecastRepository;

        public EnergyConsumptionPredictor(IConfiguration configuration, MLContext mLContext, IEnergyConsumptionForecastRepository energyConsumptionForecastRepository)
        {
            _configuration = configuration;
            _mLContext = mLContext;
            _energyConsumptionForecastRepository = energyConsumptionForecastRepository;
        }
        public async Task TrainModel()
        {
            IDataView data = LoadData();
            var dataSplit = _mLContext.Data.TrainTestSplit(data, testFraction: 0.2, seed: 1);
            var testData = dataSplit.TestSet;
            var trainData = dataSplit.TrainSet;

            var models = new List<(ITransformer model, string name)>
            {
                BuildAndTrainSdcaModel(trainData),
                BuildAndTrainFastTreeModel(trainData),
                BuildAndTrainLightGbmModel(trainData),
            };

            (var bestModel, var modelName) = EvaluateModels(models, testData);

            SaveModel(data.Schema, bestModel, modelName);
        }

        IDataView LoadData()
        {
            string connectionString = _configuration.GetConnectionString("EnergyConsumptionDB");
            string sqlQuery = @"
            SELECT EC.DateTime,
                   CAST(EC.Amount AS real) AS Amount, 
                   CAST(WC.Temperature AS real) as Temperature, 
                   WC.Conditions
            FROM EnergyConsumption EC
            JOIN WeatherConditions WC ON EC.DateTime = WC.DateTime
            ORDER BY EC.DateTime";

            DatabaseLoader loader = _mLContext.Data.CreateDatabaseLoader<InputData>();

            DatabaseSource dbSource = new DatabaseSource(SqlClientFactory.Instance, connectionString, sqlQuery);

            IDataView data = loader.Load(dbSource);

            var pipeline = _mLContext.Transforms.CustomMapping<InputData, TransformedInputData>(
                (input, output) =>
                {
                    output.Month = input.DateTime.Month;
                    output.Day = input.DateTime.Day;
                    output.Hour = input.DateTime.Hour;

                },
                contractName: null);

            var transformedData = pipeline.Fit(data).Transform(data);

            return transformedData;
        }

        void SaveModel(DataViewSchema dataViewSchema, ITransformer model, string modelName)
        {
            var modelPath = Path.Combine(Environment.CurrentDirectory, "Data", "EnergyConsumptionModel.zip");
            var metadataPath = Path.Combine(Environment.CurrentDirectory, "Data", "ModelMetadata.json");

            _mLContext.Model.Save(model, dataViewSchema, modelPath);

            var metadata = new ModelMetadata
            {
                ModelName = modelName,
                TrainedDate = DateTime.Now
            };

            var json = JsonConvert.SerializeObject(metadata);

            File.WriteAllText(metadataPath, json);
        }

        (ITransformer, string) EvaluateModels(List<(ITransformer model, string name)> models, IDataView testData)
        {
            ITransformer bestModel = null;
            string bestModelName = null;
            double bestR2 = double.MinValue;

            foreach (var (model, name) in models)
            {
                var predictions = model.Transform(testData);
                var metrics = _mLContext.Regression.Evaluate(predictions, labelColumnName: "Amount");

                Console.WriteLine($"{name} - R-Squared: {metrics.RSquared}");
                Console.WriteLine($"{name} - RMSE: {metrics.RootMeanSquaredError}");

                if (metrics.RSquared > bestR2)
                {
                    bestR2 = metrics.RSquared;
                    bestModel = model;
                    bestModelName = name;
                }
            }

            return (bestModel, bestModelName);
        }


        public async Task<List<OutputData>> Predict(List<PredictingData> data)
        {
            List<OutputData> result = new List<OutputData>();

            var modelPath = Path.Combine(Environment.CurrentDirectory, "Data", "EnergyConsumptionModel.zip");
            ITransformer trainedModel = _mLContext.Model.Load(modelPath, out var modelInputSchema);
            var predEngine = _mLContext.Model.CreatePredictionEngine<PredictingData, OutputData>(trainedModel);

            var metadataPath = Path.Combine(Environment.CurrentDirectory, "Data", "ModelMetadata.json");
            var metadataJson = File.ReadAllText(metadataPath);
            var modelMetadata = JsonConvert.DeserializeObject<ModelMetadata>(metadataJson);

            foreach (var d in data)
            {
                var prediction = predEngine.Predict(d);
                result.Add(prediction);

                Console.WriteLine(prediction.PredictedAmount);

                EnergyConsumptionForecast forecast = new EnergyConsumptionForecast
                {
                    DateTime = d.DateTime,
                    PredictedAmount = (decimal)prediction.PredictedAmount,
                    Model = modelMetadata.ModelName
                };

                await _energyConsumptionForecastRepository.AddForecast(forecast);
            }

            await _energyConsumptionForecastRepository.Save();

            return result;
        }

        (ITransformer, string) BuildAndTrainSdcaModel(IDataView trainData)
        {
            var pipeline = _mLContext.Transforms.Categorical.OneHotEncoding("ConditionsEncoded", "Conditions")
                .Append(_mLContext.Transforms.Concatenate("Features", "Temperature", "ConditionsEncoded", "Day", "Month", "Hour"))
                .Append(_mLContext.Regression.Trainers.Sdca("Amount"));

            return (pipeline.Fit(trainData), "SDCA");
        }

        (ITransformer, string) BuildAndTrainFastTreeModel(IDataView trainData)
        {
            var pipeline = _mLContext.Transforms.Categorical.OneHotEncoding("ConditionsEncoded", "Conditions")
                .Append(_mLContext.Transforms.Concatenate("Features", "Temperature", "ConditionsEncoded", "Day", "Month", "Hour"))
                .Append(_mLContext.Regression.Trainers.FastTree("Amount"));

            return (pipeline.Fit(trainData), "FastTree");
        }

        (ITransformer, string) BuildAndTrainLightGbmModel(IDataView trainData)
        {
            var pipeline = _mLContext.Transforms.Categorical.OneHotEncoding("ConditionsEncoded", "Conditions")
                .Append(_mLContext.Transforms.Concatenate("Features", "Temperature", "ConditionsEncoded", "Day", "Month", "Hour"))
                .Append(_mLContext.Regression.Trainers.LightGbm("Amount"));

            return (pipeline.Fit(trainData), "LightGBM");
        }
    }
}
