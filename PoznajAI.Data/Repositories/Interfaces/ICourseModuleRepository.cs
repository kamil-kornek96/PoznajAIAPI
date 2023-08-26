using PoznajAI.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoznajAI.Data.Repositories.Interfaces
{
    public interface ICourseModuleRepository
    {
        Task<CourseModule> GetModuleById(int moduleId);
        Task<List<CourseModule>> GetAllModules();
        Task<CourseModule> AddModule(CourseModule module);
        Task<bool> UpdateModule(CourseModule module);
        Task DeleteModule(int moduleId);
    }
}
