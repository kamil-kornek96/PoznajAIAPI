using PoznajAI.Models.LessonComment;

public interface ILessonCommentService
{
    Task<LessonCommentDto> AddComment(LessonCommentCreateDto comment);
    Task DeleteComment(int commentId);
    Task<LessonCommentDto> GetCommentById(int commentId);
    Task<List<LessonCommentDto>> GetCommentsForLesson(int lessonId);
    Task<bool> UpdateComment(LessonCommentUpdateDto comment);
}