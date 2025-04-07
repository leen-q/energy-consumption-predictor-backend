using Microsoft.ML.Data;

namespace EnergyConsumptionAPI.MachineLearning.Models
{
    public class OutputData
    {
        [ColumnName("Score")]
        public float PredictedAmount { get; set; }
    }
}
