using EnergyConsumptionAPI.Entities;

namespace EnergyConsumptionAPI.Repository
{
    public interface IEnergyConsumptionForecastRepository
    {
        Task<List<EnergyConsumptionForecast>> GetAllEnergyForecastData();
        Task AddForecast(EnergyConsumptionForecast forecast);
        Task UpdateForecast(EnergyConsumptionForecast forecast);
        Task RemoveForecast(int id);
        Task Save();
    }
}
