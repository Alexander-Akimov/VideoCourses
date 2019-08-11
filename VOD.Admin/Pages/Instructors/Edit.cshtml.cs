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

namespace VOD.Admin.Pages.Instructors
{
    [Authorize(Roles = "Admin")]
    public class EditModel : PageModel
    {
        [BindProperty]
        public InstructorDTO Input { get; set; } = new InstructorDTO();

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
                Input = await _adminService.SingleAsync<Instructor, InstructorDTO>(
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
                var succeeded = await _adminService.UpdateAsync<InstructorDTO, Instructor>(Input);
                if (succeeded)
                {
                    Alert = $"Updated Instructor: {Input.Name}.";
                    return RedirectToPage("Index");
                }                
            }
            return Page();
        }
    }
}