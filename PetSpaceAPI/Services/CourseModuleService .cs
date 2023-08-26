using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using PoznajAI.Data.Models;
using PoznajAI.Data.Repositories.Interfaces;
using PoznajAI.Models.CourseModule;

public class CourseModuleService : ICourseModuleService
{
    private readonly ICourseModuleRepository _moduleRepository;
    private readonly IMapper _mapper;

    public CourseModuleService(ICourseModuleRepository moduleRepository, IMapper mapper)
    {
        _moduleRepository = moduleRepository;
        _mapper = mapper;
    }

    public async Task<CourseModuleDto> GetModuleById(int moduleId)
    {
        var module = await _moduleRepository.GetModuleById(moduleId);
        return _mapper.Map<CourseModuleDto>(module);
    }

    public async Task<List<CourseModuleDto>> GetAllModules()
    {
        var modules = await _moduleRepository.GetAllModules();
        return _mapper.Map<List<CourseModuleDto>>(modules);
    }

    public async Task<CourseModuleDto> AddModule(CourseModuleCreateDto module)
    {
        var moduleToAdd = _mapper.Map<CourseModule>(module);
        return _mapper.Map<CourseModuleDto>(await _moduleRepository.AddModule(moduleToAdd));
    }

    public async Task<bool> UpdateModule(CourseModuleUpdateDto module)
    {
        var moduleToUpdate = _mapper.Map<CourseModule>(module);
        return await _moduleRepository.UpdateModule(moduleToUpdate);
    }

    public async Task DeleteModule(int moduleId)
    {
        await _moduleRepository.DeleteModule(moduleId);
    }
}
