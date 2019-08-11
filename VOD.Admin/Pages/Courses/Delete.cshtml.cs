using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using VOD.Common.Services;
using VOD.Database.Services;
using VOD.Domain.DTOModles.Admin;

namespace VOD.Admin.Pages.Courses
{
    [Authorize(Roles = "Admin")]
    public class DeleteModel : PageModel
    {
        [BindProperty]
        public CourseDTO Input { get; set; } = new CourseDTO();

        [TempData]
        public string Alert { get; set; }

        private readonly IAdminCoursesService _adminCoursesService;

        public DeleteModel(IAdminCoursesService adminCoursesService)
        {
            _adminCoursesService = adminCoursesService;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                Input = await _adminCoursesService.SingleAsync(id);
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
                var succeeded = await _adminCoursesService.DeleteAsync(id);
                if (succeeded)
                {
                    // Message sent back to the Index Razor Page.
                    Alert = $"Deleted Course: {Input.Title}.";
                    return RedirectToPage("Index");
                }
            }
            // Something failed, redisplay the form.
            Input = await _adminCoursesService.SingleAsync(id);
            return Page();
        }
    }
}