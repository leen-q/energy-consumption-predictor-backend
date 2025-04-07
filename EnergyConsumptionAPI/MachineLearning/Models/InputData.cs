using Microsoft.ML.Data;

namespace EnergyConsumptionAPI.MachineLearning.Models
{
    public class InputData
    {
        [LoadColumn(0)]
        public DateTime DateTime { get; set; }
        [LoadColumn(1)]
        public float Amount { get; set; }
        [LoadColumn(2)]
        public float Temperature { get; set; }
        [LoadColumn(3)]
        public string? Conditions { get; set; }

    }
}
