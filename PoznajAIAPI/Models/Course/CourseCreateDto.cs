using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace PoznajAI.Models.Course
{
    public class CourseCreateDto
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
    }
}