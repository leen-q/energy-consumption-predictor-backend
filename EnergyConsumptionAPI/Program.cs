using EnergyConsumptionAPI.Entities;
using EnergyConsumptionAPI.Repository;
using Microsoft.EntityFrameworkCore;

namespace EnergyConsumptionAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddDbContext<EnergyConsumptionDbContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("EnergyConsumptionDB")));

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddScoped<IEnergyConsumptionRepository, EnergyConsumptionRepository>();
            builder.Services.AddScoped<IEnergyConsumptionForecastRepository, EnergyConsumptionForecastRepository>();
            builder.Services.AddScoped<IWeatherConditionRepository, WeatherConditionRepository>();

            var app = builder.Build();

            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}