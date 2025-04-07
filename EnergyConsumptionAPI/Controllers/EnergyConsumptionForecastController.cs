using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EnergyConsumptionAPI.Entities;
using EnergyConsumptionAPI.Repository;

namespace EnergyConsumptionAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnergyConsumptionForecastController : ControllerBase
    {
        private IEnergyConsumptionForecastRepository _energyConsumptionForecastRepository;

        public EnergyConsumptionForecastController(IEnergyConsumptionForecastRepository energyConsumptionForecastRepository)
        {
            _energyConsumptionForecastRepository = energyConsumptionForecastRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<EnergyConsumptionForecast>>> GetAllEnergyForecastData()
        {
            return await _energyConsumptionForecastRepository.GetAllEnergyForecastData();
        }

        [HttpPost]
        public async Task AddForecast(EnergyConsumptionForecast forecast)
        {
            await _energyConsumptionForecastRepository.AddForecast(forecast);
            await _energyConsumptionForecastRepository.Save();
        }

        [HttpPut]
        public async Task UpdateForecst(EnergyConsumptionForecast forecast)
        {
            await _energyConsumptionForecastRepository.UpdateForecast(forecast);
            await _energyConsumptionForecastRepository.Save();
        }

        [HttpDelete("{id}")]
        public async Task DeleteForecast(int id)
        {
            await _energyConsumptionForecastRepository.RemoveForecast(id);
            await _energyConsumptionForecastRepository.Save();
        }
    }
}
