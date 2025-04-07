using System;
using System.Collections.Generic;

namespace EnergyConsumptionAPI.Entities
{
    public partial class EnergyConsumption
    {
        public int ConsumptionId { get; set; }
        public decimal Amount { get; set; }
        public DateTime DateTime { get; set; }

        public virtual WeatherCondition WeatherCondition { get; set; } = null!;
    }
}
