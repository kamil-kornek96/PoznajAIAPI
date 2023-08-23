using Microsoft.EntityFrameworkCore;
using PetSpace.Data.Data;
using PetSpace.Data.Models;
using PetSpace.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<double> GetTotalQuantityForUser(int userId)
        {
            return await _dbContext.FoodInventories
                .Where(fi => fi.UserId == userId)
                .SumAsync(fi => fi.QuantityInGrams);
        }

        public async Task Add(FoodInventory inventory)
        {
            await _dbContext.FoodInventories.AddAsync(inventory);
            await _dbContext.SaveChangesAsync();
        }

        public void Update(FoodInventory inventory)
        {
            _dbContext.FoodInventories.Update(inventory);
            _dbContext.SaveChanges();
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
