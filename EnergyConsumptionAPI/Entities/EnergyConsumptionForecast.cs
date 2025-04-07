using System;
using System.Collections.Generic;

namespace EnergyConsumptionAPI.Entities
{
    public partial class EnergyConsumptionForecast
    {
        public int ForecastId { get; set; }
        public DateTime DateTime { get; set; }
        public decimal PredictedAmount { get; set; }
        public string? Model { get; set; }
    }
}
