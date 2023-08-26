using Microsoft.EntityFrameworkCore;
using PoznajAI.Data.Data;
using PoznajAI.Data.Models;
using PoznajAI.Data.Repositories.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class LessonRatingRepository : ILessonRatingRepository
{
    private readonly AppDbContext _context;

    public LessonRatingRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<LessonRating> GetRatingById(int ratingId)
    {
        return await _context.LessonRatings.FirstOrDefaultAsync(lr => lr.Id == ratingId);
    }

    public async Task<List<LessonRating>> GetAllRatingsForLesson(int lessonId)
    {
        return await _context.LessonRatings
            .Where(lr => lr.LessonId == lessonId)
            .ToListAsync();
    }

    public async Task<LessonRating> AddRating(LessonRating rating)
    {
        _context.LessonRatings.Add(rating);
        await _context.SaveChangesAsync();
        return rating;
    }

    public async Task<bool> UpdateRating(LessonRating rating)
    {
        _context.LessonRatings.Update(rating);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task DeleteRating(int ratingId)
    {
        var rating = await _context.LessonRatings.FirstOrDefaultAsync(lr => lr.Id == ratingId);
        if (rating != null)
        {
            _context.LessonRatings.Remove(rating);
            await _context.SaveChangesAsync();
        }
    }
}
