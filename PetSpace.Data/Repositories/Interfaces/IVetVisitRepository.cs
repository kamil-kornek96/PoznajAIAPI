using PetSpace.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetSpace.Data.Repositories.Interfaces
{
    public interface IVetVisitRepository
    {
        Task<VetVisit> GetVetVisitById(int visitId);
        Task Add(VetVisit visit);
        void Update(VetVisit visit);
        Task Delete(int visitId);
    }
}
