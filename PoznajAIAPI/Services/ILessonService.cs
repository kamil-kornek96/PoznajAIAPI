namespace PoznajAI.Services
{
    public interface ILessonService
    {
        Task CreateLesson(CreateLessonDto lessonDto);
        Task<LessonDto> GetLessonById(Guid lessonId);
    }
}