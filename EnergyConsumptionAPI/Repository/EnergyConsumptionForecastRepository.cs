using EnergyConsumptionAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace EnergyConsumptionAPI.Repository
{
    public class EnergyConsumptionForecastRepository : IEnergyConsumptionForecastRepository
    {
        private EnergyConsumptionDbContext _context;

        public EnergyConsumptionForecastRepository(EnergyConsumptionDbContext context)
        {
            _context = context;
        }

        public async Task<List<EnergyConsumptionForecast>> GetAllEnergyForecastData()
        {
            return await _context.EnergyConsumptionForecasts.ToListAsync();
        }

        public async Task AddForecast(EnergyConsumptionForecast forecast)
        {
            await _context.EnergyConsumptionForecasts.AddAsync(forecast);
        }

        public async Task UpdateForecast(EnergyConsumptionForecast forecast)
        {
            _context.Entry(forecast).State = EntityState.Modified;
        }

        public async Task RemoveForecast(int id)
        {
            var data = await _context.EnergyConsumptionForecasts.FindAsync(id);
            _context.EnergyConsumptionForecasts.Remove(data);
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
