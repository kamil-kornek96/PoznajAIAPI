using AutoMapper;
using PetSpace.Data.Models;
using PetSpaceAPI.Models.Auth;
using PetSpaceAPI.Models.Feeding;
using PetSpaceAPI.Models.FoodConsumption;
using PetSpaceAPI.Models.FoodInventory;
using PetSpaceAPI.Models.Pet;
using PetSpaceAPI.Models.User;
using PetSpaceAPI.Models.VetVisit;

namespace PetSpaceAPI.Configuration
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<RegisterRequestDto, UserDto>().ReverseMap();
            CreateMap<Pet, PetDto>().ReverseMap();
            CreateMap<Feeding, FeedingDto>().ReverseMap();
            CreateMap<VetVisit, VetVisitDto>().ReverseMap();
            CreateMap<FoodConsumption, FoodConsumptionDto>().ReverseMap();
            CreateMap<FoodInventory, FoodInventoryDto>().ReverseMap();
        }
    }
}

