using PoznajAI.Models.LessonAssignment;

public interface ILessonAssignmentService
{
    Task<LessonAssignmentDto> AddAssignment(LessonAssignmentCreateDto assignment);
    Task DeleteAssignment(int assignmentId);
    Task<List<LessonAssignmentDto>> GetAllAssignmentsForLesson(int lessonId);
    Task<LessonAssignmentDto> GetAssignmentById(int assignmentId);
    Task<bool> UpdateAssignment(LessonAssignmentUpdateDto assignment);
}