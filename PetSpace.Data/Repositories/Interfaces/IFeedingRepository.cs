using PetSpace.Data.Models;

namespace PetSpace.Data.Repositories.Interfaces
{
    public interface IFeedingRepository
    {
        Task<Feeding> GetFeedingById(int feedingId);
        Task<Feeding> GetLatestFeedingForPet(int petId);
        IQueryable<Feeding> GetFeedingsForPet(int petId);
        Task<double> GetTotalAmountFedForPet(int petId);
        Task<Feeding> Add(Feeding feeding);
        Task<bool> Update(Feeding feeding);
        Task Delete(int feedingId);
    }
}
