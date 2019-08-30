using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using VOD.Domain.Interfaces;

using VOD.Domain.DTOModles.Admin;
using VOD.Domain.Entities;

namespace VOD.Admin.Pages.Courses
{
    [Authorize(Roles = "Admin")]
    public class DeleteModel : PageModel
    {
        [BindProperty]
        public CourseDTO Input { get; set; } = new CourseDTO();

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
                Input = await _adminService.SingleAsync<Course, CourseDTO>(c => c.Id.Equals(id), c => c.Instructor);
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
                var succeeded = await _adminService.DeleteAsync<Course>(c => c.Id.Equals(id));
                if (succeeded)
                {
                    // Message sent back to the Index Razor Page.
                    Alert = $"Deleted Course: {Input.Title}.";
                    return RedirectToPage("Index");
                }
            }
            // Something failed, redisplay the form.
            Input = await _adminService.SingleAsync<Course, CourseDTO>(c => c.Id.Equals(id), c => c.Instructor);
            return Page();
        }
    }
}