using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using PoznajAI.Data.Models;
using PoznajAI.Services;
using PoznajAI.Models.LessonComment;

namespace PoznajAI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LessonCommentsController : ControllerBase
    {
        private readonly ILessonCommentService _commentService;

        public LessonCommentsController(ILessonCommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpGet("{commentId}")]
        public async Task<ActionResult<LessonCommentDto>> GetCommentById(int commentId)
        {
            var comment = await _commentService.GetCommentById(commentId);

            if (comment == null)
            {
                return NotFound();
            }

            return Ok(comment);
        }

        [HttpGet("lesson/{lessonId}")]
        public async Task<ActionResult<List<LessonCommentDto>>> GetCommentsForLesson(int lessonId)
        {
            var comments = await _commentService.GetCommentsForLesson(lessonId);
            return Ok(comments);
        }

        [HttpPost]
        public async Task<ActionResult> AddComment([FromBody] LessonCommentCreateDto comment)
        {
            if (comment == null)
            {
                return BadRequest();
            }

            var commentDto = await _commentService.AddComment(comment);
            return CreatedAtAction(nameof(GetCommentById), new { commentId = commentDto.Id }, commentDto);
        }

        [HttpPut("{commentId}")]
        public async Task<ActionResult> UpdateComment(int commentId, [FromBody] LessonCommentUpdateDto comment)
        {
            if (comment == null || comment.Id != commentId)
            {
                return BadRequest();
            }

            await _commentService.UpdateComment(comment);
            return NoContent();
        }

        [HttpDelete("{commentId}")]
        public async Task<ActionResult> DeleteComment(int commentId)
        {
            await _commentService.DeleteComment(commentId);
            return NoContent();
        }
    }
}
