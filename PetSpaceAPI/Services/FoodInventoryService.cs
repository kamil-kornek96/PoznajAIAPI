using AutoMapper;
using PetSpace.Data.Models;
using PetSpace.Data.Repositories.Interfaces;
using PetSpaceAPI.Models.FoodInventory;

namespace PetSpaceAPI.Services
{
    public class FoodInventoryService : IFoodInventoryService
    {
        private readonly IFoodInventoryRepository _foodInventoryRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<FoodInventoryService> _logger;

        public FoodInventoryService(IFoodInventoryRepository foodInventoryRepository, IMapper mapper, ILogger<FoodInventoryService> logger)
        {
            _foodInventoryRepository = foodInventoryRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<FoodInventoryDto>> GetFoodInventoryForUser(int userId)
        {
            var foodInventory = _foodInventoryRepository.GetFoodInventoryForUser(userId);
            return _mapper.Map<IEnumerable<FoodInventoryDto>>(foodInventory);
        }

        public async Task<FoodInventoryDto> GetFoodInventoryById(int foodInventoryId)
        {
            var foodInventory = await _foodInventoryRepository.GetFoodInventoryById(foodInventoryId);
            return _mapper.Map<FoodInventoryDto>(foodInventory);
        }

        public async Task<FoodInventoryDto> CreateFoodInventory(FoodInventoryCreateDto foodInventoryDto)
        {
            var foodInventory = _mapper.Map<FoodInventory>(foodInventoryDto);
            foodInventory = await _foodInventoryRepository.Add(foodInventory);

            _logger.LogInformation("Food inventory created: {@foodInventory}", foodInventory);

            return _mapper.Map<FoodInventoryDto>(foodInventory);
        }


        public async Task UpdateFoodInventory(FoodInventoryUpdateDto foodInventoryDto)
        {
            var existingFoodInventory = await _foodInventoryRepository.GetFoodInventoryById(foodInventoryDto.Id);
            if (existingFoodInventory == null)
            {
                throw new ArgumentException($"Food inventory with ID {foodInventoryDto.Id} not found");
            }

            _mapper.Map(foodInventoryDto, existingFoodInventory);

            await _foodInventoryRepository.Update(existingFoodInventory);
            _logger.LogInformation("Food inventory updated: {@foodInventory}", existingFoodInventory);
        }

        public async Task DeleteFoodInventory(int foodInventoryId)
        {
            var foodInventory = await _foodInventoryRepository.GetFoodInventoryById(foodInventoryId);
            if (foodInventory == null)
            {
                throw new ArgumentException($"Food inventory with ID {foodInventoryId} not found");
            }

            await _foodInventoryRepository.Delete(foodInventoryId);
            _logger.LogInformation("Food inventory deleted: {@foodInventory}", foodInventory);
        }
    }
}
