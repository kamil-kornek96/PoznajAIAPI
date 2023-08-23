using PetSpace.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetSpace.Data.Repositories.Interfaces
{
    public interface IPetRepository
    {
        Task<Pet> GetPetById(int petId);
        Task<IEnumerable<Pet>> GetPetsByUserId(int userId);
        Task Add(Pet pet);
        void Update(Pet pet);
        Task Delete(int petId);
    }
}
