using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using PoznajAI.Data.Models;
using PoznajAI.Services;
using PoznajAI.Models.LessonAssignment;

namespace PoznajAI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LessonAssignmentsController : ControllerBase
    {
        private readonly ILessonAssignmentService _assignmentService;

        public LessonAssignmentsController(ILessonAssignmentService assignmentService)
        {
            _assignmentService = assignmentService;
        }

        [HttpGet("{assignmentId}")]
        public async Task<ActionResult<LessonAssignmentDto>> GetAssignmentById(int assignmentId)
        {
            var assignment = await _assignmentService.GetAssignmentById(assignmentId);

            if (assignment == null)
            {
                return NotFound();
            }

            return Ok(assignment);
        }

        [HttpGet("lesson/{lessonId}")]
        public async Task<ActionResult<List<LessonAssignmentDto>>> GetAllAssignmentsForLesson(int lessonId)
        {
            var assignments = await _assignmentService.GetAllAssignmentsForLesson(lessonId);
            return Ok(assignments);
        }

        [HttpPost]
        public async Task<ActionResult> AddAssignment([FromBody] LessonAssignmentCreateDto assignment)
        {
            if (assignment == null)
            {
                return BadRequest();
            }

            var assignmentDto = await _assignmentService.AddAssignment(assignment);
            return CreatedAtAction(nameof(GetAssignmentById), new { assignmentId = assignmentDto.Id }, assignmentDto);
        }

        [HttpPut("{assignmentId}")]
        public async Task<ActionResult> UpdateAssignment(int assignmentId, [FromBody] LessonAssignmentUpdateDto assignment)
        {
            if (assignment == null || assignment.Id != assignmentId)
            {
                return BadRequest();
            }

            await _assignmentService.UpdateAssignment(assignment);
            return NoContent();
        }

        [HttpDelete("{assignmentId}")]
        public async Task<ActionResult> DeleteAssignment(int assignmentId)
        {
            await _assignmentService.DeleteAssignment(assignmentId);
            return NoContent();
        }
    }
}
