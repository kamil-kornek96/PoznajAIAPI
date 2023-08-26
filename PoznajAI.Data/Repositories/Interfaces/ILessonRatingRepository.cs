using PoznajAI.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoznajAI.Data.Repositories.Interfaces
{
    public interface ILessonRatingRepository
    {
        Task<LessonRating> GetRatingById(int ratingId);
        Task<List<LessonRating>> GetAllRatingsForLesson(int lessonId);
        Task<LessonRating> AddRating(LessonRating rating);
        Task<bool> UpdateRating(LessonRating rating);
        Task DeleteRating(int ratingId);
    }
}
