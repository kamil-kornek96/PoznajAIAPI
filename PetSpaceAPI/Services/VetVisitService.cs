using AutoMapper;
using PetSpace.Data.Models;
using PetSpace.Data.Repositories.Interfaces;
using PetSpaceAPI.Models.VetVisit;

namespace PetSpaceAPI.Services
{
    public class VetVisitService : IVetVisitService
    {
        private readonly IVetVisitRepository _vetVisitRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<VetVisitService> _logger;

        public VetVisitService(IVetVisitRepository vetVisitRepository, IMapper mapper, ILogger<VetVisitService> logger)
        {
            _vetVisitRepository = vetVisitRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<VetVisitDto>> GetVetVisitsForPet(int petId)
        {
            var vetVisits = _vetVisitRepository.GetVetVisitsForPet(petId);
            return _mapper.Map<IEnumerable<VetVisitDto>>(vetVisits);
        }

        public async Task<VetVisitDto> GetVetVisitById(int vetVisitId)
        {
            var vetVisit = await _vetVisitRepository.GetVetVisitById(vetVisitId);
            return _mapper.Map<VetVisitDto>(vetVisit);
        }

        public async Task CreateVetVisit(VetVisitDto vetVisitDto)
        {
            var vetVisit = _mapper.Map<VetVisit>(vetVisitDto);
            await _vetVisitRepository.Add(vetVisit);
            _logger.LogInformation("Vet visit created: {@vetVisit}", vetVisit);
        }

        public async Task UpdateVetVisit(VetVisitDto vetVisitDto)
        {
            var existingVetVisit = await _vetVisitRepository.GetVetVisitById(vetVisitDto.Id);
            if (existingVetVisit == null)
            {
                throw new ArgumentException($"Vet visit with ID {vetVisitDto.Id} not found");
            }

            _mapper.Map(vetVisitDto, existingVetVisit);

            await _vetVisitRepository.Update(existingVetVisit);
            _logger.LogInformation("Vet visit updated: {@vetVisit}", existingVetVisit);
        }

        public async Task DeleteVetVisit(int vetVisitId)
        {
            var vetVisit = await _vetVisitRepository.GetVetVisitById(vetVisitId);
            if (vetVisit == null)
            {
                throw new ArgumentException($"Vet visit with ID {vetVisitId} not found");
            }

            await _vetVisitRepository.Delete(vetVisitId);
            _logger.LogInformation("Vet visit deleted: {@vetVisit}", vetVisit);
        }
    }
}
