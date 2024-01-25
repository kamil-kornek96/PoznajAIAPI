using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using PoznajAI.Models;
using PoznajAI.Models.Lesson;
using PoznajAI.Services;
using Serilog;

namespace PoznajAI.Controllers
{
    [ApiController]
    [EnableCors("DefaultPolicy")]
    [Route("api/lesson")]
    public class LessonController : ControllerBase
    {
        private readonly ILessonService _lessonService;
        private readonly ICourseService _courseService;

        public LessonController(ILessonService lessonService, ICourseService courseService)
        {
            _lessonService = lessonService;
            _courseService = courseService;
        }

        /// <summary>
        /// Retrieve a lesson with a specified identifier.
        /// </summary>
        /// <param name="id">Lesson identifier.</param>
        /// <returns>Returns the lesson with the specified identifier or an appropriate error message if not found.</returns>
        /// <response code="200">If lesson exist</response>
        /// <response code="404">If the lesson not found</response>
        /// <response code="500">If there was an issue while retrieving the lesson.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(DefaultResponse<LessonDetailsDto>), 200)]
        [ProducesResponseType(typeof(DefaultResponse<object>), 404)]
        [ProducesResponseType(typeof(DefaultResponse<object>), 500)]
        public async Task<ActionResult<LessonDetailsDto>> GetLessonById(Guid id)
        {
            try
            {
                var lesson = await _lessonService.GetLessonById(id);

                if (lesson == null)
                {
                    Log.Warning("Lesson with ID {Id} not found.", id);
                    return NotFound(new DefaultResponse<object>(404, "Lesson not found.", false));
                }

                return Ok(new DefaultResponse<LessonDetailsDto>(200, "Lesson successfuly retrieved.", false, lesson));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Problem fetching lesson data with ID: {Id}", id);
                return StatusCode(500, new DefaultResponse<object>(500, "Error fetching lesson data.", false));
            }
        }

        /// <summary>
        /// Create a new lesson.
        /// </summary>
        /// <param name="lessonDto">Lesson data to create.</param>
        /// <returns>Returns a response with status 201 on success, 404 if the associated course is not found, or 500 on server error.</returns>
        /// <response code="201">If the lesson was created. Returns newly added lesson</response>
        /// <response code="404">If the associated course not found for the lesson.</response>
        /// <response code="400">If there was any validation errors</response>
        /// <response code="500">If there was an issue while creating the lesson.</response>
        [HttpPost]
        [ProducesResponseType(typeof(DefaultResponse<LessonDto>), 201)]
        [ProducesResponseType(typeof(DefaultResponse<object>), 404)]
        [ProducesResponseType(typeof(DefaultResponse<object>), 500)]
        public async Task<ActionResult> CreateLesson(CreateLessonDto lessonDto)
        {
            try
            {
                var course = await _courseService.GetCourseById(lessonDto.CourseId);

                if (course == null)
                {
                    Log.Warning("Course with ID {Id} not found while attempting to add a lesson.", lessonDto.CourseId);
                    return NotFound(new DefaultResponse<object>(404, "Associated course not found for the lesson.", false));
                }

                var createdLesson = await _lessonService.CreateLesson(lessonDto);

                return CreatedAtAction(nameof(GetLessonById), new { id = createdLesson.Id },
                    new DefaultResponse<LessonDto>(201, "Lesson created", true, createdLesson));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error creating lesson.");
                return StatusCode(500, new DefaultResponse<object>(500, "Error creating lesson.", false));
            }
        }

        /// <summary>
        /// Update a lesson with a specified identifier.
        /// </summary>
        /// <param name="id">Lesson identifier to update.</param>
        /// <param name="lessonDto">New lesson data.</param>
        /// <returns>Returns a response with status 200 on success, 404 if the lesson to update is not found, or 500 on server error.</returns>
        /// <response code="200">If lesson was updated</response>
        /// <response code="404">If lesson to update was not found</response>
        /// <response code="500">If there was an issue while updating the lesson.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(DefaultResponse<object>), 200)]
        [ProducesResponseType(typeof(DefaultResponse<object>), 404)]
        [ProducesResponseType(typeof(DefaultResponse<object>), 500)]
        public async Task<ActionResult> UpdateLesson(Guid id, UpdateLessonDto lessonDto)
        {
            try
            {
                var existingLesson = await _lessonService.GetLessonById(id);

                if (existingLesson == null)
                {
                    Log.Warning("Lesson with ID {Id} not found for update.", id);
                    return NotFound(new DefaultResponse<object>(404, "Lesson to update not found.", false));
                }

                lessonDto.Id = id;
                await _lessonService.UpdateLesson(id, lessonDto);

                return Ok(new DefaultResponse<object>(200, "Lesson updated.", true));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error updating lesson with ID: {Id}", id);
                return StatusCode(500, new DefaultResponse<object>(500, "Error updating lesson.", false));
            }
        }

        /// <summary>
        /// Delete a lesson with a specified identifier.
        /// </summary>
        /// <param name="id">Lesson identifier to delete.</param>
        /// <returns>Returns a response with status 200 on success, 404 if the lesson to delete is not found, or 500 on server error.</returns>
        /// <response code="200">If lesson was deleted</response>
        /// <response code="404">If lesson to delete was not found</response>
        /// <response code="500">If there was an issue while deleting the lesson.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(DefaultResponse<object>), 200)]
        [ProducesResponseType(typeof(DefaultResponse<object>), 404)]
        [ProducesResponseType(typeof(DefaultResponse<object>), 500)]
        public async Task<ActionResult> DeleteLesson(Guid id)
        {
            try
            {
                var success = await _lessonService.DeleteLesson(id);

                if (!success)
                {
                    Log.Warning("Lesson with ID {Id} not found for deletion.", id);
                    return NotFound(new DefaultResponse<object>(404, "Lesson to delete not found.", false));
                }

                return Ok(new DefaultResponse<object>(200, "Lesson deleted.", true));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error deleting lesson with ID: {Id}", id);
                return StatusCode(500, new DefaultResponse<object>(500, "Error deleting lesson.", false));
            }
        }
    }
}
