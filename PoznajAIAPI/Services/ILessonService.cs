namespace PoznajAI.Services
{
    public interface ILessonService
    {
        Task CreateLesson(CreateLessonDto lessonDto);
        Task UpdateLesson(UpdateLessonDto lessonDto);
        Task<LessonDto> GetLessonById(Guid lessonId);
    }
}