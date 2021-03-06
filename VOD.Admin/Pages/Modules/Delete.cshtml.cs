using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using VOD.Domain.Interfaces;

using VOD.Domain.DTOModles.Admin;
using VOD.Domain.Entities;

namespace VOD.Admin.Pages.Modules
{
    [Authorize(Roles = "Admin")]
    public class DeleteModel : PageModel
    {
        [BindProperty]
        public ModuleDTO Input { get; set; } = new ModuleDTO();

        [TempData]
        public string Alert { get; set; }

        private readonly IAdminService _adminService;

        public DeleteModel(IAdminService adminService)
        {
            _adminService = adminService;
        }

        public async Task<IActionResult> OnGetAsync(int id, int courseId)
        {
            try
            {
                Input = await _adminService.SingleAsync<Module, ModuleDTO>(
                    m => m.Id.Equals(id) && m.CourseId.Equals(courseId));
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
            int id = Input.Id, courseId = Input.CourseId;
            if (ModelState.IsValid)
            {
                var succeeded = await _adminService.DeleteAsync<Module>(
                    m => m.Id.Equals(id) && m.CourseId.Equals(courseId));
                if (succeeded)
                {
                    // Message sent back to the Index Razor Page.
                    Alert = $"Deleted Module: {Input.Title}.";
                    return RedirectToPage("Index");
                }
            }
            // Something failed, redisplay the form.
            Input = await _adminService.SingleAsync<Module, ModuleDTO>(
                   m => m.Id.Equals(id) && m.CourseId.Equals(courseId));
            return Page();
        }
    }
}