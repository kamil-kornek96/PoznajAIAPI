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

        public async Task<IEnumerable<Pet>> GetPetsByUserId(int userId)
        {
            return await _dbContext.Pets
                .Where(p => p.User.Any(u => u.Id == userId))
                .ToListAsync();
        }

        public async Task Add(Pet pet)
        {
            await _dbContext.Pets.AddAsync(pet);
            await _dbContext.SaveChangesAsync();
        }

        public void Update(Pet pet)
        {
            _dbContext.Pets.Update(pet);
            _dbContext.SaveChanges();
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
