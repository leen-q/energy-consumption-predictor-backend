using EnergyConsumptionAPI.Entities;

namespace EnergyConsumptionAPI.Repository
{
    public interface IEnergyConsumptionRepository : IDisposable
    {
        Task<List<EnergyConsumption>> GetAllEnergyData();
        Task<EnergyConsumption> GetEnergyById(int id);
        Task AddEnergyData(EnergyConsumption energyConsumption);
        Task UpdateEnergyData(EnergyConsumption energyConsumption);
        Task RemoveEnergyData(int id);
        Task Save();
    }
}
