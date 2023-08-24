using Microsoft.EntityFrameworkCore;
using PetSpace.Data.Data;
using PetSpace.Data.Models;
using PetSpace.Data.Repositories.Interfaces;

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

        public IQueryable<FoodConsumption> GetFoodConsumptionsForPet(int consumptionId)
        {
            return _dbContext.FoodConsumptions.Where(f => f.PetId == consumptionId);
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

        public async Task<bool> Update(FoodConsumption consumption)
        {
            _dbContext.FoodConsumptions.Update(consumption);
            return _dbContext.SaveChanges() > 0;
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
