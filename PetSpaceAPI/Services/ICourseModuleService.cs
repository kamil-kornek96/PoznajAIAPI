using PoznajAI.Models.CourseModule;

public interface ICourseModuleService
{
    Task<CourseModuleDto> AddModule(CourseModuleCreateDto module);
    Task DeleteModule(int moduleId);
    Task<List<CourseModuleDto>> GetAllModules();
    Task<CourseModuleDto> GetModuleById(int moduleId);
    Task<bool> UpdateModule(CourseModuleUpdateDto module);
}