using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VOD.Common.Services;
using VOD.Database.Services;
using VOD.Domain.DTOModles.Admin;
using VOD.Domain.Entities;

namespace VOD.Admin.Pages.Courses
{
    [Authorize(Roles = "Admin")]
    public class CreateModel : PageModel
    {
        [BindProperty]
        public InstructorDTO Input { get; set; } = new InstructorDTO();

        [TempData]
        public string Alert { get; set; }

        private readonly IAdminService _adminService;
        public CreateModel(IAdminService adminService)
        {
            _adminService = adminService;
        }
        /*  public async Task<IActionResult> OnGetAsync()
          {
              return Page();
          }*/
        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var succeeded = (await _adminService.CreateAsync<InstructorDTO, Instructor>(Input)) > 0;
                if (succeeded)
                {
                    // Message sent back to the Index Razor Page.
                    Alert = $"Created a new Instructor for {Input.Name}.";
                    return RedirectToPage("Index");
                }                
            }
            // Something failed, redisplay the form.
            return Page();
        }
    }
}