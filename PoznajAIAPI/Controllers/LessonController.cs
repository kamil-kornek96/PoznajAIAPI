using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using PoznajAI.Services;
using System;
using System.Threading.Tasks;

namespace PoznajAI.Controllers
{
    [ApiController]
    [EnableCors("AllowLocalhost4200")]
    [Route("api/[controller]")]
    public class LessonController : ControllerBase
    {
        private readonly ILessonService _lessonService;
        private readonly ICourseService _courseService;

        public LessonController(ILessonService lessonService, ICourseService courseService)
        {
            _lessonService = lessonService;
            _courseService = courseService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LessonDto>> GetLessonById(Guid id)
        {
            try
            {
                var lesson = await _lessonService.GetLessonById(id);

                if (lesson == null)
                {
                    return NotFound(new { message = "Lesson not found." });
                }

                return Ok(lesson);
            }
            catch (Exception ex)
            {
                // Obsługa błędów: zaloguj błąd lub zwróć bardziej odpowiedni kod błędu HTTP.
                return StatusCode(500, new { message = "An error occurred while fetching the lesson." });
            }
        }

        [HttpPost]
        public async Task<ActionResult> CreateLesson(CreateLessonDto lessonDto)
        {
            try
            {
                var course = await _courseService.GetCourseById(lessonDto.CourseId);

                if (course == null)
                {
                    return NotFound(new { message = "Course not found." });
                }

                var createdLessonId = await _lessonService.CreateLesson(lessonDto);

                // Zamiast "Lesson created successfully.", możesz zwracać status HTTP 201 (Created) z nagłówkiem "Location" zawierającym URL do nowo utworzonej lekcji.
                return CreatedAtAction(nameof(GetLessonById), new { id = createdLessonId }, new { message = "Lesson created successfully." });
            }
            catch (Exception ex)
            {
                // Obsługa błędów: zaloguj błąd lub zwróć bardziej odpowiedni kod błędu HTTP.
                return StatusCode(500, new { message = "An error occurred while creating the lesson." });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateLesson(Guid id, UpdateLessonDto lessonDto)
        {
            try
            {
                var existingLesson = await _lessonService.GetLessonById(id);

                if (existingLesson == null)
                {
                    return NotFound(new { message = "Lesson not found." });
                }

                lessonDto.Id = id;
                await _lessonService.UpdateLesson(lessonDto);

                // Zamiast "Lesson created successfully.", możesz zwracać status HTTP 204 (No Content) lub inny odpowiedni status.
                return NoContent();
            }
            catch (Exception ex)
            {
                // Obsługa błędów: zaloguj błąd lub zwróć bardziej odpowiedni kod błędu HTTP.
                return StatusCode(500, new { message = "An error occurred while updating the lesson." });
            }
        }
    }
}
