using PetSpace.Data.Models;

namespace PetSpace.Data.Repositories.Interfaces
{
    public interface IFoodInventoryRepository
    {
        Task<FoodInventory> GetFoodInventoryById(int inventoryId);
        IQueryable<FoodInventory> GetFoodInventoryForUser(int userId);
        Task<double> GetTotalQuantityForUser(int userId);
        Task<FoodInventory> Add(FoodInventory foodInventory);
        Task<bool> Update(FoodInventory inventory);
        Task Delete(int inventoryId);
    }
}
