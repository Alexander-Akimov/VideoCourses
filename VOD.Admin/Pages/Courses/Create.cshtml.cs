using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VOD.Domain.DTOModles.Admin;
using VOD.Domain.Entities;
using VOD.Common.Extensions;
using VOD.Domain.Interfaces;
using VOD.Grpc.Common.Services;
using VOD.Domain.Interfaces.Services;

namespace VOD.Admin.Pages.Courses
{
    [Authorize(Roles = "Admin")]
    public class CreateModel : PageModel
    {
        [BindProperty]
        public CourseDTO Input { get; set; } = new CourseDTO();

        [TempData]
        public string Alert { get; set; }

        //private readonly IAdminService _adminService;
        //private readonly IAdminGrpcClientService _adminGrpcService;
        private readonly IGenericAdminService<CourseDTO> _coursesService;
        private readonly IGenericAdminService<InstructorDTO> _instructorService;

        public CreateModel(IGenericAdminService<CourseDTO> coursesService,
            IGenericAdminService<InstructorDTO> instructorService
            //IAdminService adminService, IAdminGrpcClientService adminGrpcService
            )
        {
            //_adminGrpcService = adminGrpcService;
            //_adminService = adminService;
            _coursesService = coursesService;
            _instructorService = instructorService;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                ViewData["Instructors"] = (await _instructorService.GetAsync())
                    .ToSelectList("Id", "Name");
                return Page();
            }
            catch
            {
                Alert = "You do not have access to this page.";
                return RedirectToPage("/Index");
            }
        }
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var succeeded = (await _coursesService.CreateAsync(Input)) > 0;
                    if (succeeded)
                    {
                        // Message sent back to the Index Razor Page.
                        Alert = $"Created a new Course for {Input.Title}.";
                        return RedirectToPage("Index");
                    }
                }
                // Something failed, redisplay the form.
                ViewData["Instructors"] = (await _instructorService.GetAsync())
                        .ToSelectList("Id", "Name");
                return Page();
            }
            catch
            {
                Alert = "You do not have access to this page.";
                return RedirectToPage("/Index");
            }
        }
    }
}