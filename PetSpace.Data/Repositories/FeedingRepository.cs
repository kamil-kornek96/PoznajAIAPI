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
    public class FeedingRepository : IFeedingRepository
    {
        private readonly AppDbContext _dbContext;

        public FeedingRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Feeding> GetFeedingById(int feedingId)
        {
            return await _dbContext.Feedings.FindAsync(feedingId);
        }

        public async Task<Feeding> GetLatestFeedingForPet(int petId)
        {
            return await _dbContext.Feedings
                .Where(f => f.PetId == petId)
                .OrderByDescending(f => f.FeedingTime)
                .FirstOrDefaultAsync();
        }

        public async Task<double> GetTotalAmountFedForPet(int petId)
        {
            return await _dbContext.Feedings
                .Where(f => f.PetId == petId)
                .SumAsync(f => f.AmountInGrams);
        }

        public async Task Add(Feeding feeding)
        {
            await _dbContext.Feedings.AddAsync(feeding);
            await _dbContext.SaveChangesAsync();
        }

        public void Update(Feeding feeding)
        {
            _dbContext.Feedings.Update(feeding);
            _dbContext.SaveChanges();
        }

        public async Task Delete(int feedingId)
        {
            var feeding = await _dbContext.Feedings.FindAsync(feedingId);
            if (feeding != null)
            {
                _dbContext.Feedings.Remove(feeding);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
