namespace PoznajAI.Services
{
    public interface ILessonService
    {
        Task<Guid> CreateLesson(CreateLessonDto lessonDto);
        Task<bool> DeleteLesson(Guid lessonId);
        Task<LessonDetailsDto> GetLessonById(Guid lessonId);
        Task UpdateLesson(UpdateLessonDto lessonDto);
    }
}