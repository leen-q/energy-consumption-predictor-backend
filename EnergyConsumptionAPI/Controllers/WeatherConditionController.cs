using EnergyConsumptionAPI.Entities;
using EnergyConsumptionAPI.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EnergyConsumptionAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherConditionController : ControllerBase
    {
        private IWeatherConditionRepository _weatherConditionRepository;

        public WeatherConditionController(IWeatherConditionRepository weatherConditionRepository)
        {
            _weatherConditionRepository = weatherConditionRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<WeatherCondition>>> GetAllWeatherData()
        {
            return await _weatherConditionRepository.GetAllWeatherData();
        }

        [HttpPost]
        public async Task AddWeatherData(WeatherCondition weatherData)
        {
            await _weatherConditionRepository.AddWeatherData(weatherData);
            await _weatherConditionRepository.Save();
        }

        [HttpPut]
        public async Task UpdateWeatherData(WeatherCondition weatherData)
        {
            await _weatherConditionRepository.UpdateWeatherData(weatherData);
            await _weatherConditionRepository.Save();
        }

        [HttpDelete("{id}")]
        public async Task DeleteWeatherData(int id)
        {
            await _weatherConditionRepository.RemoveWeatherData(id);
            await _weatherConditionRepository.Save();
        }
    }
}
