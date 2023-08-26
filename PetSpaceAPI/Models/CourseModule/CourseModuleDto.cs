using PoznajAI.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace PoznajAI.Models.CourseModule
{
    public class CourseModuleDto
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

    }
}
