using AutoMapper;
using PetSpace.Data.Models;
using PetSpace.Data.Repositories.Interfaces;
using PetSpaceAPI.Models.Pet;

namespace PetSpaceAPI.Services
{
    public class PetService : IPetService
    {
        private readonly IPetRepository _petRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<PetService> _logger;

        public PetService(IPetRepository petRepository, IMapper mapper, ILogger<PetService> logger)
        {
            _petRepository = petRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<PetDto>> GetPetsForUser(int userId)
        {
            var pets = _petRepository.GetPetsByUserId(userId);
            return _mapper.Map<IEnumerable<PetDto>>(pets);
        }

        public async Task<PetDto> GetPetById(int petId)
        {
            var pet = await _petRepository.GetPetById(petId);
            return _mapper.Map<PetDto>(pet);
        }

        public async Task CreatePet(PetDto petDto)
        {
            var pet = _mapper.Map<Pet>(petDto);
            await _petRepository.Add(pet);
            _logger.LogInformation("Pet created: {@pet}", pet);
        }

        public async Task UpdatePet(PetDto petDto)
        {
            var existingPet = await _petRepository.GetPetById(petDto.Id);
            if (existingPet == null)
            {
                throw new ArgumentException($"Pet with ID {petDto.Id} not found");
            }

            _mapper.Map(petDto, existingPet);

            await _petRepository.Update(existingPet);
            _logger.LogInformation("Pet updated: {@pet}", existingPet);
        }

        public async Task DeletePet(int petId)
        {
            var pet = await _petRepository.GetPetById(petId);
            if (pet == null)
            {
                throw new ArgumentException($"Pet with ID {petId} not found");
            }

            await _petRepository.Delete(petId);
            _logger.LogInformation("Pet deleted: {@pet}", pet);
        }
    }
}
