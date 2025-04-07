using EnergyConsumptionAPI.Entities;

namespace EnergyConsumptionAPI.Repository
{
    public interface IWeatherConditionRepository
    {
        Task<List<WeatherCondition>> GetAllWeatherData();
        Task AddWeatherData(WeatherCondition weatherData);
        Task UpdateWeatherData(WeatherCondition weatherData);
        Task RemoveWeatherData(int id);
        Task Save();
    }
}
