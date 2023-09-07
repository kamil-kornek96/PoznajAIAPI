using PoznajAI.Data.Models;

namespace PoznajAI.Data.Repositories
{
    public interface ILessonRepository
    {
        Task CreateLesson(Lesson lesson);
        Task DeleteLesson(Guid lessonId);
        Task<IEnumerable<Lesson>> GetAllLessonsByCourse(Guid courseId);
        Task UpdateLesson(Lesson lesson);
    }
}