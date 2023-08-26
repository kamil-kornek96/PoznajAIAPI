using PoznajAI.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoznajAI.Data.Repositories.Interfaces
{
    public interface ILessonCommentRepository
    {
        Task<LessonComment> GetCommentById(int commentId);
        Task<List<LessonComment>> GetCommentsForLesson(int lessonId);
        Task<LessonComment> AddComment(LessonComment comment);
        Task<bool> UpdateComment(LessonComment comment);
        Task DeleteComment(int commentId);
    }
}
