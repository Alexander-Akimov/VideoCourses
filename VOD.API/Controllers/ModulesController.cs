using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using VOD.Domain.Interfaces;
using VOD.Domain.DTOModles.Admin;
using VOD.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using VOD.Common.Constants;

namespace VOD.API.Controllers
{
    [Route("api/courses/{courseId}/[controller]")]
    [ApiController]
    [Authorize(Policy = nameof(Roles.Admin))]
    public class ModulesController : ControllerBase
    {
        private readonly IAdminService _adminService;
        private readonly LinkGenerator _linkGenerator;
        private readonly ILogger _logger;
        public ModulesController(IAdminService adminService, LinkGenerator linkGenerator, ILogger<ModulesController> logger)
        {
            this._adminService = adminService;
            this._linkGenerator = linkGenerator;
            this._logger = logger;
        }
        [HttpGet]
        public async Task<ActionResult<List<ModuleDTO>>> Get(int courseId, bool include = false)
        {
            try
            {
                Expression<Func<Module, bool>> whereExpr = m => m.CourseId.Equals(courseId);

                var dtos = await _adminService.GetAsync<Module, ModuleDTO>(
                    whereExpr: courseId.Equals(0) ? null : whereExpr,
                    include: include ? new Expression<Func<Module, object>>[] { m => m.Videos, m => m.Downloads } : null);

                return dtos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Filure");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ModuleDTO>> Get(int courseId, int id, bool include = false)
        {
            try
            {
                var dto = await _adminService.SingleAsync<Module, ModuleDTO>(
                    whereExpr: m => courseId.Equals(0) ? m.Id.Equals(id) : m.Id.Equals(id) && m.CourseId.Equals(courseId),
                    include: include ? new Expression<Func<Module, object>>[] { m => m.Videos, m => m.Downloads } : null);

                if (dto == null) return NotFound();
                return dto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Filure");
            }
        }

        [HttpPost]
        public async Task<ActionResult<List<ModuleDTO>>> Post(int courseId, ModuleDTO model)
        {
            if (courseId.Equals(0)) courseId = model.CourseId;
            if (model == null) return BadRequest("No entity provided");

            try
            {              
                var courseExist = await _adminService.AnyAsync<Course>(c => c.Id.Equals(courseId));
                if (!courseExist) return NotFound("Could not find related entity");

                if (!model.CourseId.Equals(courseId)) return BadRequest("Differing ids");

                var id = await _adminService.CreateAsync<ModuleDTO, Module>(model);
                if (id < 1) return BadRequest("Unable to add the entity");

                var dto = await _adminService.SingleAsync<Module, ModuleDTO>(m => m.CourseId.Equals(courseId) && m.Id.Equals(id));
                if (dto == null) return BadRequest("Unable to add the entity");

                var uri = _linkGenerator.GetPathByAction("Get", "Modules", new { id, courseId });

                return Created(uri, dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to add the entity");
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int courseId, int id, ModuleDTO model)
        {
            if (model == null) return BadRequest("No entity provided");
            if (!id.Equals(model.Id)) return BadRequest("Differing ids");

            try
            {
                var courseExist = await _adminService.AnyAsync<Course>(c => c.Id.Equals(courseId));
                if (!courseExist) return NotFound("Could not find related entity");

                var exist = await _adminService.AnyAsync<Module>(c => c.Id.Equals(id));
                if (!exist) return NotFound("Could not find entity");

                if (await _adminService.UpdateAsync<ModuleDTO, Module>(model))
                    return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to update the entity");
            }
            return BadRequest("Unable to update the entity");
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int courseId, int id)
        {
            try
            {
                var exist = await _adminService.AnyAsync<Module>(m => m.Id.Equals(id) && m.CourseId.Equals(courseId));
                if (!exist) return BadRequest("Could not find entity");

                if (await _adminService.DeleteAsync<Module>(m => m.Id.Equals(id) && m.CourseId.Equals(courseId)))
                    return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to delete the entity");
            }
            return BadRequest("Failed to delete the entity");
        }
    }
}