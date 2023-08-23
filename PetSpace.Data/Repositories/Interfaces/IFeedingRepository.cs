using PetSpace.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetSpace.Data.Repositories
{
    public interface IFeedingRepository
    {
        Task<Feeding> GetFeedingById(int feedingId);
        Task<Feeding> GetLatestFeedingForPet(int petId);
        Task<double> GetTotalAmountFedForPet(int petId);
        Task Add(Feeding feeding);
        void Update(Feeding feeding);
        Task Delete(int feedingId);
    }
}
