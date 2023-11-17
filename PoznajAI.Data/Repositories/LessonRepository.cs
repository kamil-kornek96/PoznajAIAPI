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

        public async Task<Lesson> CreateLesson(Lesson lesson)
        {
            try
            {
                _dbContext.Lessons.Add(lesson);
                await _dbContext.SaveChangesAsync();
                return lesson;
            }
            catch (Exception ex)
            {
                throw new Exception("Nie udało się utworzyć lekcji.", ex);
            }
        }

        public async Task<Lesson> UpdateLesson(Lesson lesson)
        {
            _dbContext.Entry(lesson).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return lesson;
        }

        public async Task<bool> DeleteLesson(Guid lessonId)
        {
            var lesson = await _dbContext.Lessons.FindAsync(lessonId);
            if (lesson != null)
            {
                _dbContext.Lessons.Remove(lesson);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
