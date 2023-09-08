using PoznajAI.Data.Models;

namespace PoznajAI.Data.Repositories
{
    public interface ILessonRepository
    {
        Task CreateLesson(Lesson lesson);
        Task DeleteLesson(Guid lessonId);
        Task<Lesson> GetLessonById(Guid lessonId);
        Task UpdateLesson(Lesson lesson);
    }
}