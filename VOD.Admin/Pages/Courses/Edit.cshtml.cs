using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VOD.Domain.Interfaces;

using VOD.Domain.DTOModles.Admin;
using VOD.Domain.Entities;
using VOD.Common.Extensions;

namespace VOD.Admin.Pages.Courses
{
    [Authorize(Roles = "Admin")]
    public class EditModel : PageModel
    {
        [BindProperty]
        public CourseDTO Input { get; set; } = new CourseDTO();

        [TempData]
        public string Alert { get; set; }

        private readonly IAdminService _adminService;

        public EditModel(IAdminService adminService)
        {
            _adminService = adminService;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                Alert = String.Empty;
                ViewData["Instructors"] = (await _adminService.GetAsync<Instructor, InstructorDTO>())
                    .ToSelectList("Id", "Name");
                Input = await _adminService.SingleAsync<Course, CourseDTO>(
                    instr => instr.Id.Equals(id));

                return Page();
            }
            catch// (Exception)
            {
                Alert = "You do not have access to this page.";
                return RedirectToPage("/Index");
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var succeeded = await _adminService.UpdateAsync<CourseDTO, Course>(Input);
                if (succeeded)
                {
                    Alert = $"Updated Course: {Input.Title}.";                   
                    return RedirectToPage("Index");
                }                
            }
            ViewData["Instructors"] = (await _adminService.GetAsync<Instructor, InstructorDTO>())
                   .ToSelectList("Id", "Name");
            return Page();
        }
    }
}