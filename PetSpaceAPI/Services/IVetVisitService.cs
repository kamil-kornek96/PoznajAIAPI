using PetSpaceAPI.Models.VetVisit;

namespace PetSpaceAPI.Services
{
    public interface IVetVisitService
    {
        Task<IEnumerable<VetVisitDto>> GetVetVisitsForPet(int petId);
        Task<VetVisitDto> GetVetVisitById(int vetVisitId);
        Task CreateVetVisit(VetVisitDto vetVisitDto);
        Task UpdateVetVisit(VetVisitDto vetVisitDto);
        Task DeleteVetVisit(int vetVisitId);
    }
}
