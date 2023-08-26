using Microsoft.EntityFrameworkCore;
using PoznajAI.Data.Data;
using PoznajAI.Data.Models;
using PoznajAI.Data.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class LessonAssignmentRepository : ILessonAssignmentRepository
{
    private readonly AppDbContext _context;

    public LessonAssignmentRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<LessonAssignment> GetAssignmentById(int assignmentId)
    {
        return await _context.LessonAssignments.FirstOrDefaultAsync(la => la.Id == assignmentId);
    }

    public async Task<List<LessonAssignment>> GetAllAssignmentsForLesson(int lessonId)
    {
        return await _context.LessonAssignments
            .Where(la => la.LessonId == lessonId)
            .ToListAsync();
    }

    public async Task<LessonAssignment> AddAssignment(LessonAssignment assignment)
    {
        _context.LessonAssignments.Add(assignment);
        await _context.SaveChangesAsync();
        return assignment;
    }

    public async Task<bool> UpdateAssignment(LessonAssignment assignment)
    {
        _context.LessonAssignments.Update(assignment);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task DeleteAssignment(int assignmentId)
    {
        var assignment = await _context.LessonAssignments.FirstOrDefaultAsync(la => la.Id == assignmentId);
        if (assignment != null)
        {
            _context.LessonAssignments.Remove(assignment);
            await _context.SaveChangesAsync();
        }
    }
}
