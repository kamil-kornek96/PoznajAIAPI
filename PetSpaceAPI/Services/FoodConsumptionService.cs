using AutoMapper;
using PetSpace.Data.Models;
using PetSpace.Data.Repositories.Interfaces;
using PetSpaceAPI.Models.FoodConsumption;

namespace PetSpaceAPI.Services
{
    public class FoodConsumptionService : IFoodConsumptionService
    {
        private readonly IFoodConsumptionRepository _foodConsumptionRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<FoodConsumptionService> _logger;

        public FoodConsumptionService(IFoodConsumptionRepository foodConsumptionRepository, IMapper mapper, ILogger<FoodConsumptionService> logger)
        {
            _foodConsumptionRepository = foodConsumptionRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<FoodConsumptionDto>> GetFoodConsumptionsForPet(int petId)
        {
            var foodConsumptions = _foodConsumptionRepository.GetFoodConsumptionsForPet(petId);
            return _mapper.Map<IEnumerable<FoodConsumptionDto>>(foodConsumptions);
        }

        public async Task<FoodConsumptionDto> GetFoodConsumptionById(int foodConsumptionId)
        {
            var foodConsumption = await _foodConsumptionRepository.GetFoodConsumptionById(foodConsumptionId);
            return _mapper.Map<FoodConsumptionDto>(foodConsumption);
        }

        public async Task CreateFoodConsumption(FoodConsumptionDto foodConsumptionDto)
        {
            var foodConsumption = _mapper.Map<FoodConsumption>(foodConsumptionDto);
            await _foodConsumptionRepository.Add(foodConsumption);
            _logger.LogInformation("Food consumption created: {@foodConsumption}", foodConsumption);
        }

        public async Task UpdateFoodConsumption(FoodConsumptionDto foodConsumptionDto)
        {
            var existingFoodConsumption = await _foodConsumptionRepository.GetFoodConsumptionById(foodConsumptionDto.Id);
            if (existingFoodConsumption == null)
            {
                throw new ArgumentException($"Food consumption with ID {foodConsumptionDto.Id} not found");
            }

            _mapper.Map(foodConsumptionDto, existingFoodConsumption);

            await _foodConsumptionRepository.Update(existingFoodConsumption);
            _logger.LogInformation("Food consumption updated: {@foodConsumption}", existingFoodConsumption);
        }

        public async Task DeleteFoodConsumption(int foodConsumptionId)
        {
            var foodConsumption = await _foodConsumptionRepository.GetFoodConsumptionById(foodConsumptionId);
            if (foodConsumption == null)
            {
                throw new ArgumentException($"Food consumption with ID {foodConsumptionId} not found");
            }

            await _foodConsumptionRepository.Delete(foodConsumptionId);
            _logger.LogInformation("Food consumption deleted: {@foodConsumption}", foodConsumption);
        }
    }
}
