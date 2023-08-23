using PetSpace.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetSpace.Data.Repositories.Interfaces
{
    public interface IFoodInventoryRepository
    {
        Task<FoodInventory> GetFoodInventoryById(int inventoryId);
        Task<double> GetTotalQuantityForUser(int userId);
        Task Add(FoodInventory inventory);
        void Update(FoodInventory inventory);
        Task Delete(int inventoryId);
    }
}
