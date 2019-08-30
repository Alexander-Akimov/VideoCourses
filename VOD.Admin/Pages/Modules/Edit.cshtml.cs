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

namespace VOD.Admin.Pages.Modules
{
    [Authorize(Roles = "Admin")]
    public class EditModel : PageModel
    {
        [BindProperty]
        public ModuleDTO Input { get; set; } = new ModuleDTO();

        [TempData]
        public string Alert { get; set; }

        private readonly IAdminService _adminService;

        public EditModel(IAdminService adminService)
        {
            _adminService = adminService;
        }

        public async Task<IActionResult> OnGetAsync(int id, int courseId)
        {
            try
            {
                Alert = String.Empty;
                ViewData["Courses"] = (await _adminService.GetAsync<Course, CourseDTO>())
                    .ToSelectList("Id", "Title");
                Input = await _adminService.SingleAsync<Module, ModuleDTO>(
                    m => m.Id.Equals(id) && m.CourseId.Equals(courseId));

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
                var succeeded = await _adminService.UpdateAsync<ModuleDTO, Module>(Input);
                if (succeeded)
                {
                    Alert = $"Updated Module: {Input.Title}.";
                    return RedirectToPage("Index");
                }
            }
            ViewData["Courses"] = (await _adminService.GetAsync<Course, CourseDTO>())
                   .ToSelectList("Id", "Title");
            return Page();
        }
    }
}