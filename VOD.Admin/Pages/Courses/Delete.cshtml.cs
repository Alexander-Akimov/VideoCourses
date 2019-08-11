using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using VOD.Common.Services;
using VOD.Database.Services;
using VOD.Domain.DTOModles.Admin;
using VOD.Domain.Entities;

namespace VOD.Admin.Pages.Courses
{
    [Authorize(Roles = "Admin")]
    public class DeleteModel : PageModel
    {
        [BindProperty]
        public InstructorDTO Input { get; set; } = new InstructorDTO();

        [TempData]
        public string Alert { get; set; }

        private readonly IAdminService _adminService;

        public DeleteModel(IAdminService adminService)
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
            catch //(Exception)
            {
                Alert = "You do not have access to this page.";
                return RedirectToPage("/Index");
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var id = Input.Id;
            if (ModelState.IsValid)
            {
                var succeeded = await _adminService.DeleteAsync<Instructor>(
                    d => d.Equals(id));
                if (succeeded)
                {
                    // Message sent back to the Index Razor Page.
                    Alert = $"Deleted Instructor: {Input.Name}.";
                    return RedirectToPage("Index");
                }
            }
            // Something failed, redisplay the form.
            Input = await _adminService.SingleAsync<Instructor, InstructorDTO>(
                    instr => instr.Id.Equals(id));
            return Page();
        }
    }
}