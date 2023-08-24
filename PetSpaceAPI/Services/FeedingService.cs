using AutoMapper;
using PetSpace.Data.Models;
using PetSpace.Data.Repositories.Interfaces;
using PetSpaceAPI.Models.Feeding;

namespace PetSpaceAPI.Services
{
    public class FeedingService : IFeedingService
    {
        private readonly IFeedingRepository _feedingRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<FeedingService> _logger;

        public FeedingService(IFeedingRepository feedingRepository, IMapper mapper, ILogger<FeedingService> logger)
        {
            _feedingRepository = feedingRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<FeedingDto>> GetFeedingsForPet(int petId)
        {
            var feedings = _feedingRepository.GetFeedingsForPet(petId);
            return _mapper.Map<IEnumerable<FeedingDto>>(feedings);
        }

        public async Task<FeedingDto> GetFeedingById(int feedingId)
        {
            var feeding = await _feedingRepository.GetFeedingById(feedingId);
            return _mapper.Map<FeedingDto>(feeding);
        }

        public async Task<FeedingDto> CreateFeeding(FeedingCreateDto feedingDto)
        {
            var feeding = _mapper.Map<Feeding>(feedingDto);
            feeding = await _feedingRepository.Add(feeding);

            _logger.LogInformation("Feeding created: {@feeding}", feeding);

            return _mapper.Map<FeedingDto>(feeding);
        }

        public async Task UpdateFeeding(FeedingUpdateDto feedingDto)
        {
            var existingFeeding = await _feedingRepository.GetFeedingById(feedingDto.Id);
            if (existingFeeding == null)
            {
                throw new ArgumentException($"Feeding with ID {feedingDto.Id} not found");
            }

            _mapper.Map(feedingDto, existingFeeding);

            await _feedingRepository.Update(existingFeeding);
            _logger.LogInformation("Feeding updated: {@feeding}", existingFeeding);
        }

        public async Task DeleteFeeding(int feedingId)
        {
            var feeding = await _feedingRepository.GetFeedingById(feedingId);
            if (feeding == null)
            {
                throw new ArgumentException($"Feeding with ID {feedingId} not found");
            }

            await _feedingRepository.Delete(feedingId);
            _logger.LogInformation("Feeding deleted: {@feeding}", feeding);
        }
    }
}
