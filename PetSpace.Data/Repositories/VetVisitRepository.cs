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

        public async Task Add(VetVisit visit)
        {
            await _dbContext.VetVisits.AddAsync(visit);
            await _dbContext.SaveChangesAsync();
        }

        public void Update(VetVisit visit)
        {
            _dbContext.VetVisits.Update(visit);
            _dbContext.SaveChanges();
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
