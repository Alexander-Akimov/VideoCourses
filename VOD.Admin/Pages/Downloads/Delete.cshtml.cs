using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using VOD.Domain.Interfaces;

using VOD.Domain.DTOModles.Admin;
using VOD.Domain.Entities;

namespace VOD.Admin.Pages.Downloads
{
    [Authorize(Roles = "Admin")]
    public class DeleteModel : PageModel
    {
        [BindProperty]
        public DownloadDTO Input { get; set; } = new DownloadDTO();

        [TempData]
        public string Alert { get; set; }

        private readonly IAdminService _adminService;

        public DeleteModel(IAdminService adminService)
        {
            _adminService = adminService;
        }

        public async Task<IActionResult> OnGetAsync(int id, int moduleId, int courseId)
        {
            try
            {
                Input = await _adminService.SingleAsync<Download, DownloadDTO>(
                      whereExpr: v => v.Id.Equals(id) && v.ModuleId.Equals(moduleId) && v.CourseId.Equals(courseId),
                      include: true);
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
            var moduleId = Input.ModuleId;
            var courseId = Input.CourseId;
            if (ModelState.IsValid)
            {
                var succeeded = await _adminService.DeleteAsync<Download>(
                    v => v.Id.Equals(id) && v.ModuleId.Equals(moduleId) && v.CourseId.Equals(courseId));
                if (succeeded)
                {
                    // Message sent back to the Index Razor Page.
                    Alert = $"Deleted Download: {Input.Title}.";
                    return RedirectToPage("Index");
                }
            }
            // Something failed, redisplay the form.
            Input = await _adminService.SingleAsync<Download, DownloadDTO>(
                     whereExpr: v => v.Id.Equals(id) && v.ModuleId.Equals(moduleId) && v.CourseId.Equals(courseId),
                     include: true);
            return Page();
        }
    }
}