using Microsoft.EntityFrameworkCore;
using PetSpace.Data.Data;
using PetSpace.Data.Models;
using PetSpace.Data.Repositories.Interfaces;

namespace PetSpace.Data.Repositories
{
    public class FoodInventoryRepository : IFoodInventoryRepository
    {
        private readonly AppDbContext _dbContext;

        public FoodInventoryRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<FoodInventory> GetFoodInventoryById(int inventoryId)
        {
            return await _dbContext.FoodInventories.FindAsync(inventoryId);
        }

        public IQueryable<FoodInventory> GetFoodInventoryForUser(int userId)
        {
            return _dbContext.FoodInventories.Where(f => f.UserId == userId);
        }

        public async Task<double> GetTotalQuantityForUser(int userId)
        {
            return await _dbContext.FoodInventories
                .Where(fi => fi.UserId == userId)
                .SumAsync(fi => fi.QuantityInGrams);
        }

        public async Task<FoodInventory> Add(FoodInventory foodInventory)
        {
            await _dbContext.FoodInventories.AddAsync(foodInventory);
            await _dbContext.SaveChangesAsync();
            return foodInventory;
        }


        public async Task<bool> Update(FoodInventory inventory)
        {
            _dbContext.FoodInventories.Update(inventory);
            return _dbContext.SaveChanges() > 0;
        }

        public async Task Delete(int inventoryId)
        {
            var inventory = await _dbContext.FoodInventories.FindAsync(inventoryId);
            if (inventory != null)
            {
                _dbContext.FoodInventories.Remove(inventory);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
