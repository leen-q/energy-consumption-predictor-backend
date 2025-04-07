using EnergyConsumptionAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace EnergyConsumptionAPI.Repository
{
    public class EnergyConsumptionRepository : IEnergyConsumptionRepository
    {

        private EnergyConsumptionDbContext _context;

        public EnergyConsumptionRepository(EnergyConsumptionDbContext context)
        {
            _context = context;
        }

        public async Task<List<EnergyConsumption>> GetAllEnergyData()
        {
            return await _context.EnergyConsumptions.ToListAsync();
        }

        public async Task<EnergyConsumption> GetEnergyById(int id)
        {
            return await _context.EnergyConsumptions.FindAsync(id);
        }

        public async Task AddEnergyData(EnergyConsumption energyConsumption)
        {
            await _context.EnergyConsumptions.AddAsync(energyConsumption);
        }

        public async Task UpdateEnergyData(EnergyConsumption energyConsumption)
        {
            _context.Entry(energyConsumption).State = EntityState.Modified;
        }

        public async Task RemoveEnergyData(int id)
        {
            var data = await _context.EnergyConsumptions.FindAsync(id);
            _context.EnergyConsumptions.Remove(data);
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
