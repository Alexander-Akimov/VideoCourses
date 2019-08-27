using System;
using System.Collections.Generic;
using System.Linq;
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
    public class InstructorsController : ControllerBase
    {
        private readonly IAdminService _adminService;
        private readonly LinkGenerator _linkGenerator;
        public InstructorsController(IAdminService adminService, LinkGenerator linkGenerator)
        {
            this._adminService = adminService;
            this._linkGenerator = linkGenerator;
        }

        [HttpGet]
        public async Task<ActionResult<List<InstructorDTO>>> Get(bool include = false)
        {
            try
            {
                return await _adminService.GetAsync<Instructor, InstructorDTO>();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Filure");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<InstructorDTO>> Get(int id)
        {
            try
            {
                var dto = await _adminService.SingleAsync<Instructor, InstructorDTO>(instr => instr.Id.Equals(id));
                if (dto == null) return NotFound();
                return dto;
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Filure");
            }
        }

        [HttpPost]
        public async Task<ActionResult<List<InstructorDTO>>> Post(InstructorDTO model)
        {
            try
            {
                if (model == null) return BadRequest("No entity provided");

                var id = await _adminService.CreateAsync<InstructorDTO, Instructor>(model);
                var dto = await _adminService.SingleAsync<Instructor, InstructorDTO>(instr => instr.Id.Equals(id));
                if (dto == null) return BadRequest("Unable to add entity");

                var uri = _linkGenerator.GetPathByAction("Get", "Instructors", new { id });

                return Created(uri, dto);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to add the entity");
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, InstructorDTO model)
        {
            try
            {
                if (model == null) return BadRequest("No entity provided");
                if (!id.Equals(model.Id)) return BadRequest("Differing ids");

                var exist = await _adminService.AnyAsync<Instructor>(instr => instr.Id.Equals(id));
                if (!exist) return NotFound("Could not find entity");

                if (await _adminService.UpdateAsync<InstructorDTO, Instructor>(model))
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
                var exist = await _adminService.AnyAsync<Instructor>(instr => instr.Id.Equals(id));
                if (!exist) return BadRequest("Could not find entity");

                if (await _adminService.DeleteAsync<Instructor>(d => d.Id.Equals(id)))
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