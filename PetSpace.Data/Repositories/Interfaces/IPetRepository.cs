using PetSpace.Data.Models;

namespace PetSpace.Data.Repositories.Interfaces
{
    public interface IPetRepository
    {
        Task<Pet> GetPetById(int petId);
        IQueryable<Pet> GetPetsByUserId(int userId);
        Task Add(Pet pet);
        Task<bool> Update(Pet pet);
        Task Delete(int petId);
    }
}
