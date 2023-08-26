using Microsoft.EntityFrameworkCore;
using PoznajAI.Data.Data;
using PoznajAI.Data.Models;
using PoznajAI.Data.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class LessonCommentRepository : ILessonCommentRepository
{
    private readonly AppDbContext _context;

    public LessonCommentRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<LessonComment> GetCommentById(int commentId)
    {
        return await _context.LessonComments.FirstOrDefaultAsync(c => c.Id == commentId);
    }

    public async Task<List<LessonComment>> GetCommentsForLesson(int lessonId)
    {
        return await _context.LessonComments
            .Where(c => c.LessonId == lessonId)
            .ToListAsync();
    }

    public async Task<LessonComment> AddComment(LessonComment comment)
    {
        _context.LessonComments.Add(comment);
        await _context.SaveChangesAsync();
        return comment;
    }

    public async Task<bool> UpdateComment(LessonComment comment)
    {
        _context.LessonComments.Update(comment);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task DeleteComment(int commentId)
    {
        var comment = await _context.LessonComments.FirstOrDefaultAsync(c => c.Id == commentId);
        if (comment != null)
        {
            _context.LessonComments.Remove(comment);
            await _context.SaveChangesAsync();
        }
    }
}
