using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetSpaceAPI.Models.FoodInventory;
using PetSpaceAPI.Services;

namespace PetSpaceAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class FoodInventoryController : ControllerBase
    {
        private readonly IFoodInventoryService _foodInventoryService;

        public FoodInventoryController(IFoodInventoryService foodInventoryService)
        {
            _foodInventoryService = foodInventoryService;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetFoodInventoryForUser(int userId)
        {
            var foodInventory = await _foodInventoryService.GetFoodInventoryForUser(userId);
            return Ok(foodInventory);
        }

        [HttpGet("{foodInventoryId}")]
        public async Task<IActionResult> GetFoodInventoryById(int foodInventoryId)
        {
            var foodInventory = await _foodInventoryService.GetFoodInventoryById(foodInventoryId);
            if (foodInventory == null)
            {
                return NotFound();
            }
            return Ok(foodInventory);
        }

        [HttpPost]
        public async Task<IActionResult> CreateFoodInventory([FromBody] FoodInventoryCreateDto foodInventoryCreateDto)
        {
            try
            {

                var foodInventoryDto = await _foodInventoryService.CreateFoodInventory(foodInventoryCreateDto);
                return CreatedAtAction(nameof(GetFoodInventoryById), new { foodInventoryId = foodInventoryDto.Id }, foodInventoryDto);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{foodInventoryId}")]
        public async Task<IActionResult> UpdateFoodInventory(int foodInventoryId, [FromBody] FoodInventoryUpdateDto foodInventoryUpdateDto)
        {
            try
            {
                foodInventoryUpdateDto.Id = foodInventoryId;
                await _foodInventoryService.UpdateFoodInventory(foodInventoryUpdateDto);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{foodInventoryId}")]
        public async Task<IActionResult> DeleteFoodInventory(int foodInventoryId)
        {
            try
            {
                await _foodInventoryService.DeleteFoodInventory(foodInventoryId);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
