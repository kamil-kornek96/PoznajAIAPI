using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using PoznajAI.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PoznajAI.Controllers
{
    [ApiController]
    [EnableCors("AllowLocalhost4200")]
    [Route("api/[controller]")]
    public class LessonController : ControllerBase
    {
        private readonly ILessonService _lessonService;

        public LessonController(ILessonService lessonService)
        {
            _lessonService = lessonService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LessonDto>> GetLessonById(Guid id)
        {
            try
            {
                var lesson = await _lessonService.GetLessonById(id);
                return Ok(lesson);
            }
            catch (Exception ex)
            {
                // Handle exceptions here (e.g., log the error).
                return StatusCode(500, new { message = "An error occurred while fetching lessons." });
            }
        }

        [HttpPost("create")]
        public async Task<ActionResult> CreateLesson(CreateLessonDto lessonDto)
        {
            try
            {
                await _lessonService.CreateLesson(lessonDto);
                return Ok(new { message = "Lesson created successfully." });
            }
            catch (Exception ex)
            {
                // Handle exceptions here (e.g., log the error).
                return StatusCode(500, new { message = "An error occurred while creating the lesson." });
            }
        }
    }
}
