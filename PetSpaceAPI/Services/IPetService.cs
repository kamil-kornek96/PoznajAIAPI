using PetSpaceAPI.Models.Pet;

namespace PetSpaceAPI.Services
{
    public interface IPetService
    {
        Task<IEnumerable<PetDto>> GetPetsForUser(int userId);
        Task<PetDto> GetPetById(int petId);
        Task CreatePet(PetDto petDto);
        Task UpdatePet(PetDto petDto);
        Task DeletePet(int petId);
    }
}
