using PoznajAI.Models.Lesson;

namespace PoznajAI.Services
{
    public interface ILessonService
    {
        Task<LessonDto> CreateLesson(CreateLessonDto lessonDto);
        Task<bool> DeleteLesson(Guid lessonId);
        Task<LessonDetailsDto> GetLessonById(Guid lessonId);
        Task<bool> UpdateLesson(Guid lessonId, UpdateLessonDto lessonDto);
    }
}