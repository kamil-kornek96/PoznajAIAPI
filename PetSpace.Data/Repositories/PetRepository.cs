using PetSpace.Data.Data;
using PetSpace.Data.Models;
using PetSpace.Data.Repositories.Interfaces;

namespace PetSpace.Data.Repositories
{
    public class PetRepository : IPetRepository
    {
        private readonly AppDbContext _dbContext;

        public PetRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Pet> GetPetById(int petId)
        {
            return await _dbContext.Pets.FindAsync(petId);
        }

        public IQueryable<Pet> GetPetsByUserId(int userId)
        {
            return _dbContext.Pets
                .Where(p => p.User.Any(u => u.Id == userId));
        }

        public async Task Add(Pet pet)
        {
            await _dbContext.Pets.AddAsync(pet);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> Update(Pet pet)
        {
            _dbContext.Pets.Update(pet);
            return _dbContext.SaveChanges() > 0;
        }

        public async Task Delete(int petId)
        {
            var pet = await _dbContext.Pets.FindAsync(petId);
            if (pet != null)
            {
                _dbContext.Pets.Remove(pet);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
