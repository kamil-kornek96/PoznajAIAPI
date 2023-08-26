using PoznajAI.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoznajAI.Data.Repositories.Interfaces
{
    public interface ILessonAssignmentRepository
    {
        Task<LessonAssignment> GetAssignmentById(int assignmentId);
        Task<List<LessonAssignment>> GetAllAssignmentsForLesson(int lessonId);
        Task<LessonAssignment> AddAssignment(LessonAssignment assignment);
        Task<bool> UpdateAssignment(LessonAssignment assignment);
        Task DeleteAssignment(int assignmentId);
    }
}
