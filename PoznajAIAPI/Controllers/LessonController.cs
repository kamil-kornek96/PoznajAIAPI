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
                    return NotFound(new { message = "Nie znaleziono lekcji." });
                }

                return Ok(lesson);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Problem z pobraniem danych lekcji." });
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
                    return NotFound(new { message = "Nie znaleziono kursu do którego miałaby być dodana lekcja." });
                }

                var createdLessonId = await _lessonService.CreateLesson(lessonDto);

                return CreatedAtAction(nameof(GetLessonById), new { id = createdLessonId }, new { message = "Dodano lekcje." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Problem z dodaniem lekcji." });
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
                    return NotFound(new { message = "Nie znaleziono lekcji do aktualizacji." });
                }

                lessonDto.Id = id;
                await _lessonService.UpdateLesson(id,lessonDto);

                return Ok(new { message = "Zaktualizowano lekcje." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Problem z aktualizacją lekcji." });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteLesson(Guid id)
        {
            try
            {
                var success = await _lessonService.DeleteLesson(id);

                if (!success)
                {
                    return NotFound(new { message = "Nie znaleziono lekcji do usunięcia." });
                }

                return Ok(new { message = "Usunięto lekcje." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Problem z usunięciem lekcji." });
            }
        }

    }
}
