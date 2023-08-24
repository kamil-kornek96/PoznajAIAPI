using PetSpace.Data.Models;

namespace PetSpace.Data.Repositories.Interfaces
{
    public interface IVetVisitRepository
    {
        Task<VetVisit> GetVetVisitById(int visitId);
        IQueryable<VetVisit> GetVetVisitsForPet(int petId);
        Task Add(VetVisit visit);
        Task<bool> Update(VetVisit visit);
        Task Delete(int visitId);
    }
}
