using PetSpace.Data.Models;

namespace PetSpace.Data.Repositories.Interfaces
{
    public interface IFoodConsumptionRepository
    {
        Task<FoodConsumption> GetFoodConsumptionById(int consumptionId);
        IQueryable<FoodConsumption> GetFoodConsumptionsForPet(int consumptionId);
        Task<double> GetTotalAmountConsumedForPet(int petId);
        Task Add(FoodConsumption consumption);
        Task<bool> Update(FoodConsumption consumption);
        Task Delete(int consumptionId);
    }
}
