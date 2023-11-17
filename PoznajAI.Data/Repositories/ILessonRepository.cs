using PoznajAI.Data.Models;

namespace PoznajAI.Data.Repositories
{
    public interface ILessonRepository
    {
        Task<Lesson> CreateLesson(Lesson lesson);
        Task<bool> DeleteLesson(Guid lessonId);
        Task<Lesson> GetLessonById(Guid lessonId);
        Task<Lesson> UpdateLesson(Lesson lesson);
    }
}