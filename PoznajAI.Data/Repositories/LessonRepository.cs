using Microsoft.EntityFrameworkCore;
using PoznajAI.Data.Data;
using PoznajAI.Data.Models;

namespace PoznajAI.Data.Repositories
{
    public class LessonRepository : ILessonRepository
    {
        private readonly AppDbContext _dbContext;

        public LessonRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Lesson> GetLessonById(Guid lessonId)
        {
            return await _dbContext.Lessons.FindAsync(lessonId);
        }

        public async Task CreateLesson(Lesson lesson)
        {
            try
            {
                await _dbContext.Lessons.AddAsync(lesson);
                await _dbContext.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                var errorMessage = ex.Message;
            }
        }

        public async Task UpdateLesson(Lesson lesson)
        {
            _dbContext.Lessons.Update(lesson);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteLesson(Guid lessonId)
        {
            var lesson = await _dbContext.Lessons.FindAsync(lessonId);
            if (lesson != null)
            {
                _dbContext.Lessons.Remove(lesson);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
