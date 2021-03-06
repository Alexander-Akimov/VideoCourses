﻿using System;
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
using VOD.Common.Extensions;
using Microsoft.AspNetCore.Authorization;
using VOD.Common.Constants;

namespace VOD.API.Controllers
{
    [Route("api/courses/{courseId}/modules/{moduleId}/[controller]")]
    [ApiController]
    [Authorize(Policy = nameof(Roles.Admin))]
    public class VideosController : ControllerBase
    {
        private readonly IAdminService _adminService;
        private readonly LinkGenerator _linkGenerator;
        private readonly ILogger _logger;
        public VideosController(IAdminService adminService, LinkGenerator linkGenerator, ILogger<VideosController> logger)
        {
            this._adminService = adminService;
            this._linkGenerator = linkGenerator;
            this._logger = logger;
        }
        [HttpGet]
        public async Task<ActionResult<List<VideoDTO>>> Get(int moduleId, int courseId, bool include = false)
        {
            try
            {
                Expression<Func<Video, bool>> whereExpr = v => v.CourseId.Equals(courseId) && v.ModuleId.Equals(moduleId);

                var dtos = await _adminService.GetAsync<Video, VideoDTO>(
                    whereExpr: courseId.Equals(0) ? null : whereExpr,
                    include: include ? new Expression<Func<Video, object>>[] { v => v.Module, v => v.Course } : null);

                return dtos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Filure");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<VideoDTO>> Get(int moduleId, int courseId, int id, bool include = false)
        {
            try
            {
                var dto = await _adminService.SingleAsync<Video, VideoDTO>(
                    whereExpr: v => courseId.Equals(0) ? v.Id.Equals(id) : v.Id.Equals(id) && v.CourseId.Equals(courseId) && v.ModuleId.Equals(moduleId),
                    include: include ? new Expression<Func<Video, object>>[] { v => v.Module, v => v.Course } : null);

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
        public async Task<ActionResult<VideoDTO>> Post(int moduleId, int courseId, VideoDTO model)
        {
            if (moduleId.Equals(0)) moduleId = model.ModuleId;
            if (courseId.Equals(0)) courseId = model.CourseId;

            if (model == null) return BadRequest("No entity provided");
            if (model.Title.IsNullOrEmptyOrWhiteSpace()) return BadRequest("Title is required");

            try
            {
                var courseExist = await _adminService.AnyAsync<Course>(c => c.Id.Equals(courseId));
                if (!courseExist) return NotFound("Could not find related entity");

                var moduleExist = await _adminService.AnyAsync<Module>(m => m.Id.Equals(moduleId) && m.CourseId.Equals(courseId));
                if (!courseExist) return NotFound("Could not find related entity");

                if (!model.CourseId.Equals(courseId)) return BadRequest("Differing ids");

                var id = await _adminService.CreateAsync<VideoDTO, Video>(model);
                if (id < 1) return BadRequest("Unable to add the entity");

                var dto = await _adminService.SingleAsync<Video, VideoDTO>(v => v.ModuleId.Equals(moduleId) && v.CourseId.Equals(courseId) && v.Id.Equals(id));
                if (dto == null) return BadRequest("Unable to add the entity");

                var uri = _linkGenerator.GetPathByAction("Get", "Videos", new { id, courseId, moduleId });

                return Created(uri, dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to add the entity");
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int moduleId, int courseId, int id, VideoDTO model)
        {
            if (model == null) return BadRequest("Missing entity");
            if (!id.Equals(model.Id)) return BadRequest("Differing ids");
            if (model.Title.IsNullOrEmptyOrWhiteSpace()) return BadRequest("Title is required");//It validates by ModelState.IsValid, doesn't it?

            try
            {
                var exist = await _adminService.AnyAsync<Module>(c => c.Id.Equals(moduleId));
                if (!exist) return NotFound("Could not find related entity");
                exist = await _adminService.AnyAsync<Course>(c => c.Id.Equals(courseId));
                if (!exist) return NotFound("Could not find related entity");
                exist = await _adminService.AnyAsync<Module>(c => c.Id.Equals(courseId) && c.Id.Equals(moduleId));
                if (!exist) return NotFound("Could not find related entity");

                exist = await _adminService.AnyAsync<Video>(c => c.Id.Equals(id));
                if (!exist) return NotFound("Could not find entity");

                if (await _adminService.UpdateAsync<VideoDTO, Video>(model))
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
        public async Task<IActionResult> Delete(int moduleId, int courseId, int id)
        {
            try
            {
                var exist = await _adminService.AnyAsync<Video>(v => v.ModuleId.Equals(moduleId) && v.Id.Equals(id) && v.CourseId.Equals(courseId));
                if (!exist) return BadRequest("Could not find entity");

                if (await _adminService.DeleteAsync<Video>(v => v.ModuleId.Equals(moduleId) && v.Id.Equals(id) && v.CourseId.Equals(courseId)))
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