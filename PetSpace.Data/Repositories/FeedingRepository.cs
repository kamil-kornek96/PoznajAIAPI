using Microsoft.EntityFrameworkCore;
using PetSpace.Data.Data;
using PetSpace.Data.Models;
using PetSpace.Data.Repositories.Interfaces;

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

        public IQueryable<Feeding> GetFeedingsForPet(int petId)
        {
            return _dbContext.Feedings.Where(f => f.PetId == petId);
        }

        public async Task<double> GetTotalAmountFedForPet(int petId)
        {
            return await _dbContext.Feedings
                .Where(f => f.PetId == petId)
                .SumAsync(f => f.AmountInGrams);
        }

        public async Task<Feeding> Add(Feeding feeding)
        {
            await _dbContext.Feedings.AddAsync(feeding);
            await _dbContext.SaveChangesAsync();
            return feeding;
        }

        public async Task<bool> Update(Feeding feeding)
        {
            _dbContext.Feedings.Update(feeding);
            return _dbContext.SaveChanges() > 0;
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
