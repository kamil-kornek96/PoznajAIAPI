using PetSpaceAPI.Models.FoodConsumption;

namespace PetSpaceAPI.Services
{
    public interface IFoodConsumptionService
    {
        Task<IEnumerable<FoodConsumptionDto>> GetFoodConsumptionsForPet(int petId);
        Task<FoodConsumptionDto> GetFoodConsumptionById(int foodConsumptionId);
        Task CreateFoodConsumption(FoodConsumptionDto foodConsumptionDto);
        Task UpdateFoodConsumption(FoodConsumptionDto foodConsumptionDto);
        Task DeleteFoodConsumption(int foodConsumptionId);
    }
}
