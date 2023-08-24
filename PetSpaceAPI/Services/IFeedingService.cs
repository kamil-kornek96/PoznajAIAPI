using PetSpaceAPI.Models.Feeding;

namespace PetSpaceAPI.Services
{
    public interface IFeedingService
    {
        Task<IEnumerable<FeedingDto>> GetFeedingsForPet(int petId);
        Task<FeedingDto> GetFeedingById(int feedingId);
        Task<FeedingDto> CreateFeeding(FeedingCreateDto feedingDto);
        Task UpdateFeeding(FeedingUpdateDto feedingDto);
        Task DeleteFeeding(int feedingId);
    }
}
