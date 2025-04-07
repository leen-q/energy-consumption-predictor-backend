namespace EnergyConsumptionAPI.MachineLearning.Models
{
    public class PredictingData
    {
        public DateTime DateTime { get; set; }
        public float Amount { get; set; }
        public float Temperature { get; set; }
        public string? Conditions { get; set; }
        public float Month => DateTime.Month;
        public float Day => DateTime.Day;
        public float Hour => DateTime.Hour;

        public override string ToString()
        {
            return $"DateTime: {DateTime}, Temp: {Temperature}, Conditions: {Conditions}, Month: {Month}, Day: {Day}, Hour: {Hour}";
        }
    }
}
