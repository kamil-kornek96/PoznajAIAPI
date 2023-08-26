using Microsoft.EntityFrameworkCore;
using PoznajAI.Data.Data;
using PoznajAI.Data.Models;
using PoznajAI.Data.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class CourseModuleRepository : ICourseModuleRepository
{
    private readonly AppDbContext _context;

    public CourseModuleRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<CourseModule> GetModuleById(int moduleId)
    {
        return await _context.CourseModules.FirstOrDefaultAsync(m => m.Id == moduleId);
    }

    public async Task<List<CourseModule>> GetAllModules()
    {
        return await _context.CourseModules.ToListAsync();
    }

    public async Task<CourseModule> AddModule(CourseModule module)
    {
        _context.CourseModules.Add(module);
        await _context.SaveChangesAsync();
        return module;
    }

    public async Task<bool> UpdateModule(CourseModule module)
    {
        _context.CourseModules.Update(module);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task DeleteModule(int moduleId)
    {
        var module = await _context.CourseModules.FirstOrDefaultAsync(m => m.Id == moduleId);
        if (module != null)
        {
            _context.CourseModules.Remove(module);
            await _context.SaveChangesAsync();
        }
    }
}
