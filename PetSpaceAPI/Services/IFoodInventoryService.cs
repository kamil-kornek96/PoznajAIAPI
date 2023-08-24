using PetSpaceAPI.Models.FoodInventory;

namespace PetSpaceAPI.Services
{
    public interface IFoodInventoryService
    {
        Task<IEnumerable<FoodInventoryDto>> GetFoodInventoryForUser(int userId);
        Task<FoodInventoryDto> GetFoodInventoryById(int foodInventoryId);
        Task<FoodInventoryDto> CreateFoodInventory(FoodInventoryCreateDto foodInventoryDto);
        Task UpdateFoodInventory(FoodInventoryUpdateDto foodInventoryDto);
        Task DeleteFoodInventory(int foodInventoryId);
    }
}
