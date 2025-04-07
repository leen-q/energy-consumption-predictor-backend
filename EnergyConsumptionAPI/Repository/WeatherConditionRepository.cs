using EnergyConsumptionAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace EnergyConsumptionAPI.Repository
{
    public class WeatherConditionRepository : IWeatherConditionRepository
    {
        private EnergyConsumptionDbContext _context;

        public WeatherConditionRepository(EnergyConsumptionDbContext context)
        {
            _context = context;
        }

        public async Task<List<WeatherCondition>> GetAllWeatherData()
        {
            return await _context.WeatherConditions.ToListAsync();
        }

        public async Task AddWeatherData(WeatherCondition weatherData)
        {
            await _context.WeatherConditions.AddAsync(weatherData);
        }

        public async Task UpdateWeatherData(WeatherCondition weatherData)
        {
            _context.Entry(weatherData).State = EntityState.Modified;
        }

        public async Task RemoveWeatherData(int id)
        {
            var data = await _context.WeatherConditions.FindAsync(id);
            _context.WeatherConditions.Remove(data);
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
