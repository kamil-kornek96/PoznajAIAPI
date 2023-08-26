using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PoznajAI.Data.Models;
using PoznajAI.Services;
using PoznajAI.Models.CourseModule;

namespace PoznajAI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourseModulesController : ControllerBase
    {
        private readonly ICourseModuleService _moduleService;

        public CourseModulesController(ICourseModuleService moduleService)
        {
            _moduleService = moduleService;
        }

        [HttpGet("{moduleId}")]
        public async Task<ActionResult<CourseModule>> GetModuleById(int moduleId)
        {
            var module = await _moduleService.GetModuleById(moduleId);

            if (module == null)
            {
                return NotFound();
            }

            return Ok(module);
        }

        [HttpGet]
        public async Task<ActionResult<List<CourseModule>>> GetAllModules()
        {
            var modules = await _moduleService.GetAllModules();
            return Ok(modules);
        }

        [HttpPost]
        public async Task<ActionResult> AddModule([FromBody] CourseModuleCreateDto module)
        {
            if (module == null)
            {
                return BadRequest();
            }

            var moduleDto = await _moduleService.AddModule(module);
            return CreatedAtAction(nameof(GetModuleById), new { moduleId = moduleDto.Id }, moduleDto);
        }

        [HttpPut("{moduleId}")]
        public async Task<ActionResult> UpdateModule(int moduleId, [FromBody] CourseModuleUpdateDto module)
        {
            if (module == null || module.Id != moduleId)
            {
                return BadRequest();
            }

            await _moduleService.UpdateModule(module);
            return NoContent();
        }

        [HttpDelete("{moduleId}")]
        public async Task<ActionResult> DeleteModule(int moduleId)
        {
            await _moduleService.DeleteModule(moduleId);
            return NoContent();
        }
    }
}
