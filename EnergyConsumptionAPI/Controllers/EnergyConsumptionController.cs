using EnergyConsumptionAPI.Entities;
using EnergyConsumptionAPI.MachineLearning;
using EnergyConsumptionAPI.Repository;
using Microsoft.AspNetCore.Mvc;

namespace EnergyConsumptionAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EnergyConsumptionController : ControllerBase
    {
        private IEnergyConsumptionRepository _energyConsumptionRepository; 

        public EnergyConsumptionController(IEnergyConsumptionRepository energyConsumptionRepository)
        {
            _energyConsumptionRepository = energyConsumptionRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<EnergyConsumption>>> GetAllEnergyData()
        {
            return await _energyConsumptionRepository.GetAllEnergyData();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EnergyConsumption>> GetEnergyDataById(int id)
        {
            return await _energyConsumptionRepository.GetEnergyById(id);
        }

        [HttpPost]
        public async Task AddEnergyData(EnergyConsumption energyConsumption)
        {
            await _energyConsumptionRepository.AddEnergyData(energyConsumption);
            await _energyConsumptionRepository.Save();
        }

        [HttpPut]
        public async Task UpdateEnergyData(EnergyConsumption energyConsumption)
        {
            await _energyConsumptionRepository.UpdateEnergyData(energyConsumption);
            await _energyConsumptionRepository.Save();
        }

        [HttpDelete("{id}")]
        public async Task DeleteEnergyData(int id)
        {
            await _energyConsumptionRepository.RemoveEnergyData(id);
            await _energyConsumptionRepository.Save();
        }
    }
}