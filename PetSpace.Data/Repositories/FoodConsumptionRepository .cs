using Microsoft.EntityFrameworkCore;
using PetSpace.Data.Data;
using PetSpace.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetSpace.Data.Repositories
{
    public class FoodConsumptionRepository : IFoodConsumptionRepository
    {
        private readonly AppDbContext _dbContext;

        public FoodConsumptionRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<FoodConsumption> GetFoodConsumptionById(int consumptionId)
        {
            return await _dbContext.FoodConsumptions.FindAsync(consumptionId);
        }

        public async Task<double> GetTotalAmountConsumedForPet(int petId)
        {
            return await _dbContext.FoodConsumptions
                .Where(fc => fc.PetId == petId)
                .SumAsync(fc => fc.AmountInGrams);
        }

        public async Task Add(FoodConsumption consumption)
        {
            await _dbContext.FoodConsumptions.AddAsync(consumption);
            await _dbContext.SaveChangesAsync();
        }

        public void Update(FoodConsumption consumption)
        {
            _dbContext.FoodConsumptions.Update(consumption);
            _dbContext.SaveChanges();
        }

        public async Task Delete(int consumptionId)
        {
            var consumption = await _dbContext.FoodConsumptions.FindAsync(consumptionId);
            if (consumption != null)
            {
                _dbContext.FoodConsumptions.Remove(consumption);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
