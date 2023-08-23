using PetSpace.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetSpace.Data.Repositories
{
    public interface IFoodConsumptionRepository
    {
        Task<FoodConsumption> GetFoodConsumptionById(int consumptionId);
        Task<double> GetTotalAmountConsumedForPet(int petId);
        Task Add(FoodConsumption consumption);
        void Update(FoodConsumption consumption);
        Task Delete(int consumptionId);
    }
}
