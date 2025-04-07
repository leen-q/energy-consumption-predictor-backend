using EnergyConsumptionAPI.MachineLearning;
using EnergyConsumptionAPI.MachineLearning.Models;
using EnergyConsumptionAPI.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ML;

namespace EnergyConsumptionAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PredictionController : ControllerBase
    {
        private IConfiguration _configuration;
        private EnergyConsumptionPredictor _predictor;
        private MLContext _mlContext;
        private IEnergyConsumptionForecastRepository _energyConsumptionForecastRepository;

        public PredictionController(IConfiguration configuration, IEnergyConsumptionForecastRepository energyConsumptionForecastRepository)
        {
            _mlContext = new MLContext();
            _configuration = configuration;
            _energyConsumptionForecastRepository = energyConsumptionForecastRepository;
            _predictor = new EnergyConsumptionPredictor(_configuration, _mlContext, _energyConsumptionForecastRepository);
        }

        [HttpGet("train")]
        public async Task Train()
        {
            await _predictor.TrainModel();
        }

        [HttpPost]
        public async Task<List<OutputData>> Predict([FromBody]List<PredictingData> data)
        {
            return await _predictor.Predict(data);
        }
    }
}
