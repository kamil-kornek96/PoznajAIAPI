using PoznajAI.Models.LessonRating;

public interface ILessonRatingService
{
    Task<LessonRatingDto> AddRating(LessonRatingCreateDto rating);
    Task DeleteRating(int ratingId);
    Task<List<LessonRatingDto>> GetAllRatingsForLesson(int lessonId);
    Task<LessonRatingDto> GetRatingById(int ratingId);
    Task<bool> UpdateRating(LessonRatingUpdateDto rating);
}