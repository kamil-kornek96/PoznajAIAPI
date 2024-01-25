using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using PoznajAI.Models;
using PoznajAI.Models.Course;
using PoznajAI.Services;
using Serilog;

namespace PoznajAI.Controllers
{
    [ApiController]
    [EnableCors("AllowLocalhost4200")]
    [Route("api/course")]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _courseService;

        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        /// <summary>
        /// Retrieves a course with a specified identifier.
        /// </summary>
        /// <param name="id">Course identifier.</param>
        /// <returns>Returns the course with the specified identifier or an appropriate error code on failure.</returns>
        /// <response code="200">Returns the course with the specified identifier.</response>
        /// <response code="404">If the course with the specified identifier is not found.</response>
        /// <response code="500">If there was an issue while retrieving the course.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(DefaultResponse<CourseDto>), 200)]
        [ProducesResponseType(typeof(DefaultResponse<object>), 404)]
        [ProducesResponseType(typeof(DefaultResponse<object>), 500)]
        public async Task<ActionResult<CourseDto>> GetCourseById(Guid id)
        {
            try
            {
                var course = await _courseService.GetCourseById(id);

                if (course == null)
                {
                    return NotFound(new DefaultResponse<object>(404, "Course not found.", false));
                }

                return Ok(new DefaultResponse<CourseDto>(200, "Courses successfuly retrieved.", true, course));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Problem with retrieving the course with ID: {Id}", id);
                return StatusCode(500, new DefaultResponse<object>(500, "Problem with retrieving the course.", false));
            }
        }

        /// <summary>
        /// Creates a new course.
        /// </summary>
        /// <param name="courseDto">Course data to create.</param>
        /// <returns>Returns a response with status 201 on success, 400 on model errors or 500 on server error.</returns>
        /// <response code="201">If the course was created. Returns newly added course</response>
        /// <response code="400">If there was any validation errors</response>
        /// <response code="500">If there was an issue while creating the course.</response>
        [HttpPost]
        [ProducesResponseType(typeof(DefaultResponse<CourseDto>), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(typeof(DefaultResponse<object>), 500)]
        public async Task<ActionResult> CreateCourse(CourseCreateDto courseDto)
        {
            try
            {
                var createdCourse = await _courseService.CreateCourse(courseDto);
                return CreatedAtAction(nameof(GetCourseById), new { id = createdCourse.Id },
                    new DefaultResponse<object>(201, "Course created", true, createdCourse));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Problem with creating a course");
                return StatusCode(500, new DefaultResponse<object>(500, "Problem with creating a course.", false));
            }
        }

        /// <summary>
        /// Updates a course with a specified identifier.
        /// </summary>
        /// <param name="id">Course identifier to update.</param>
        /// <param name="courseDto">Course data to update.</param>
        /// <returns>Returns a response with status 200 on success, 400 on bad request, 404 if the course to update is not found, or 500 on server error.</returns>
        /// <response code="200">Returns if the course was updated succesfully.</response>
        /// <response code="404">If the course with the specified identifier is not found.</response>
        /// <response code="500">If there was an issue while updating the course.</response>
        [Authorize]
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(DefaultResponse<object>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(typeof(DefaultResponse<object>), 404)]
        [ProducesResponseType(typeof(DefaultResponse<object>), 500)]
        public async Task<ActionResult> UpdateCourse(Guid id, CourseUpdateDto courseDto)
        {
            try
            {
                var success = await _courseService.UpdateCourse(id, courseDto);

                if (!success)
                {
                    return NotFound(new DefaultResponse<object>(404, "Course to update not found.", false));
                }

                return Ok(new DefaultResponse<object>(200, "Course updated.", true));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Problem with updating the course with ID: {Id}", id);
                return StatusCode(500, new DefaultResponse<object>(500, "Problem with updating the course.", false));
            }
        }

        /// <summary>
        /// Deletes a course with a specified identifier.
        /// </summary>
        /// <param name="id">Course identifier to delete.</param>
        /// <returns>Returns a response with status 200 on success, 404 if the course to delete is not found, or 500 on server error.</returns>
        /// <response code="200">Returns if the course was deleted succesfully.</response>
        /// <response code="404">If the course with the specified identifier is not found.</response>
        /// <response code="500">If there was an issue while deleting the course.</response>
        [Authorize]
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(DefaultResponse<object>), 200)]
        [ProducesResponseType(typeof(DefaultResponse<object>), 404)]
        [ProducesResponseType(typeof(DefaultResponse<object>), 500)]
        public async Task<ActionResult> DeleteCourse(Guid id)
        {
            try
            {
                var success = await _courseService.DeleteCourse(id);

                if (!success)
                {
                    return NotFound(new DefaultResponse<object>(404, "Course to delete not found.", false));
                }

                return Ok(new DefaultResponse<object>(200, "Course deleted.", true));
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Problem with deleting the course with ID: {Id}", id);
                return StatusCode(500, new DefaultResponse<object>(500, "Problem with deleting the course.", false));
            }
        }
    }
}
