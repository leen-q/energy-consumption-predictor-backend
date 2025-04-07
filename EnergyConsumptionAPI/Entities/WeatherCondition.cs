using System;
using System.Collections.Generic;

namespace EnergyConsumptionAPI.Entities
{
    public partial class WeatherCondition
    {
        public int WeatherId { get; set; }
        public decimal Temperature { get; set; }
        public string? Conditions { get; set; }
        public DateTime DateTime { get; set; }

        public virtual EnergyConsumption DateTimeNavigation { get; set; } = null!;
    }
}
