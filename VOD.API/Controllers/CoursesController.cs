using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using VOD.Common.Services;
using VOD.Domain.DTOModles.Admin;
using VOD.Domain.Entities;

namespace VOD.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly IAdminService _adminService;
        private readonly LinkGenerator _linkGenerator;
        public CoursesController(IAdminService adminService, LinkGenerator linkGenerator)
        {
            this._adminService = adminService;
            this._linkGenerator = linkGenerator;
        }
        [HttpGet]
        public async Task<ActionResult<List<CourseDTO>>> Get(bool include = false)
        {
            try
            {
                var navPropPaths = include ? new Expression<Func<Course, object>>[] { c => c.Instructor, c => c.Modules } : null ;
                return await _adminService.GetAsync<Course, CourseDTO>(navPropPaths);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Filure");
                //TODO: log
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<CourseDTO>> Get(int id, bool include = false)
        {
            try
            {
                var navPropPaths = include ? new Expression<Func<Course, object>>[] { c => c.Instructor, c => c.Modules } : null;
                var dto = await _adminService.SingleAsync<Course, CourseDTO>(c => c.Id.Equals(id), navPropPaths);
                if (dto == null) return NotFound();
                return dto;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Filure");
            }
        }

        [HttpPost]
        public async Task<ActionResult<List<CourseDTO>>> Post(CourseDTO model)
        {
            try
            {
                if (model == null) return BadRequest("No entity provided");

                var exist = await _adminService.AnyAsync<Instructor>(instr => instr.Id.Equals(model.InstructorId));
                if (!exist) return NotFound("Could not find related entity");

                var id = await _adminService.CreateAsync<CourseDTO, Course>(model);
                if (id < 1) return BadRequest("Unable to add the entity");

                var dto = await _adminService.SingleAsync<Course, CourseDTO>(c => c.Id.Equals(id));
                if (dto == null) return BadRequest("Unable to add the entity");

                var uri = _linkGenerator.GetPathByAction("Get", "Courses", new { id });

                return Created(uri, dto);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to add the entity");
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, CourseDTO model)
        {
            try
            {
                if (model == null) return BadRequest("Missing entity");
                if (!id.Equals(model.Id)) return BadRequest("Differing ids");

                var instrExist = await _adminService.AnyAsync<Instructor>(instr => instr.Id.Equals(model.InstructorId));
                if (!instrExist) return NotFound("Could not find related entity");

                var exist = await _adminService.AnyAsync<Course>(c => c.Id.Equals(id));
                if (!exist) return NotFound("Could not find entity");

                if (await _adminService.UpdateAsync<CourseDTO, Course>(model))
                    return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to update the entity");
            }
            return BadRequest("Unable to update the entity");
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var exist = await _adminService.AnyAsync<Course>(c => c.Id.Equals(id));
                if (!exist) return BadRequest("Could not find entity");

                if (await _adminService.DeleteAsync<Course>(d => d.Id.Equals(id)))
                    return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to delete the entity");
            }
            return BadRequest("Failed to delete the entity");
        }


    }
}