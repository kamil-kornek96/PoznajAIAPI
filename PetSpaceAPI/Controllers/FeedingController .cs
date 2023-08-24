using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetSpaceAPI.Models.Feeding;
using PetSpaceAPI.Services;

namespace PetSpaceAPI.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class FeedingController : ControllerBase
    {
        private readonly IFeedingService _feedingService;

        public FeedingController(IFeedingService feedingService)
        {
            _feedingService = feedingService;
        }

        [HttpGet("{feedingId}")]
        public async Task<ActionResult<FeedingDto>> GetFeedingById(int feedingId)
        {
            var feeding = await _feedingService.GetFeedingById(feedingId);
            if (feeding == null)
            {
                return NotFound();
            }
            return Ok(feeding);
        }

        [HttpGet("pet/{petId}")]
        public async Task<ActionResult<IEnumerable<FeedingDto>>> GetFeedingsForPet(int petId)
        {
            var feedings = await _feedingService.GetFeedingsForPet(petId);
            return Ok(feedings);
        }

        [HttpPost]
        public async Task<IActionResult> CreateFeeding(FeedingCreateDto feedingCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var feedingDto = await _feedingService.CreateFeeding(feedingCreateDto);
            return CreatedAtAction(nameof(GetFeedingById), new { feedingId = feedingDto.Id }, feedingDto);
        }

        [HttpPut("{feedingId}")]
        public async Task<IActionResult> UpdateFeeding(int feedingId, FeedingUpdateDto feedingUpdateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (feedingId != feedingUpdateDto.Id)
            {
                return BadRequest("Feeding ID mismatch");
            }

            try
            {
                await _feedingService.UpdateFeeding(feedingUpdateDto);
            }
            catch (ArgumentException)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{feedingId}")]
        public async Task<IActionResult> DeleteFeeding(int feedingId)
        {
            try
            {
                await _feedingService.DeleteFeeding(feedingId);
            }
            catch (ArgumentException)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
