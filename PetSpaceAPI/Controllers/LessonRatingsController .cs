using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PoznajAI.Data.Models;
using PoznajAI.Services;
using PoznajAI.Models.LessonRating;

namespace PoznajAI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LessonRatingsController : ControllerBase
    {
        private readonly ILessonRatingService _ratingService;

        public LessonRatingsController(ILessonRatingService ratingService)
        {
            _ratingService = ratingService;
        }

        [HttpGet("{ratingId}")]
        public async Task<ActionResult<LessonRatingDto>> GetRatingById(int ratingId)
        {
            var rating = await _ratingService.GetRatingById(ratingId);

            if (rating == null)
            {
                return NotFound();
            }

            return Ok(rating);
        }

        [HttpGet("lesson/{lessonId}")]
        public async Task<ActionResult<List<LessonRatingDto>>> GetAllRatingsForLesson(int lessonId)
        {
            var ratings = await _ratingService.GetAllRatingsForLesson(lessonId);
            return Ok(ratings);
        }

        [HttpPost]
        public async Task<ActionResult> AddRating([FromBody] LessonRatingCreateDto rating)
        {
            if (rating == null)
            {
                return BadRequest();
            }

            var ratingDto = await _ratingService.AddRating(rating);
            return CreatedAtAction(nameof(GetRatingById), new { ratingId = ratingDto.Id }, ratingDto);
        }

        [HttpPut("{ratingId}")]
        public async Task<ActionResult> UpdateRating(int ratingId, [FromBody] LessonRatingUpdateDto rating)
        {
            if (rating == null || rating.Id != ratingId)
            {
                return BadRequest();
            }

            await _ratingService.UpdateRating(rating);
            return NoContent();
        }

        [HttpDelete("{ratingId}")]
        public async Task<ActionResult> DeleteRating(int ratingId)
        {
            await _ratingService.DeleteRating(ratingId);
            return NoContent();
        }
    }
}
