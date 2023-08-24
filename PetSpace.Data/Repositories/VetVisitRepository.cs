using PetSpace.Data.Data;
using PetSpace.Data.Models;
using PetSpace.Data.Repositories.Interfaces;

namespace PetSpace.Data.Repositories
{
    public class VetVisitRepository : IVetVisitRepository
    {
        private readonly AppDbContext _dbContext;

        public VetVisitRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<VetVisit> GetVetVisitById(int visitId)
        {
            return await _dbContext.VetVisits.FindAsync(visitId);
        }

        public IQueryable<VetVisit> GetVetVisitsForPet(int petId)
        {
            return _dbContext.VetVisits.Where(v => v.PetId == petId);
        }

        public async Task Add(VetVisit visit)
        {
            await _dbContext.VetVisits.AddAsync(visit);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> Update(VetVisit visit)
        {
            _dbContext.VetVisits.Update(visit);
            return _dbContext.SaveChanges() > 0;
        }

        public async Task Delete(int visitId)
        {
            var visit = await _dbContext.VetVisits.FindAsync(visitId);
            if (visit != null)
            {
                _dbContext.VetVisits.Remove(visit);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
