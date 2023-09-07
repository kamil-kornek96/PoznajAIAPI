namespace PoznajAI.Services
{
    public interface ILessonService
    {
        Task CreateLesson(CreateLessonDto lessonDto);
        Task<IEnumerable<LessonDto>> GetAllLessonsForCourse(Guid courseId);
    }
}