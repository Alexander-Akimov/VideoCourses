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

namespace VOD.Admin.Pages.Downloads
{
    [Authorize(Roles = "Admin")]
    public class EditModel : PageModel
    {
        [BindProperty]
        public DownloadDTO Input { get; set; } = new DownloadDTO();

        [TempData]
        public string Alert { get; set; }

        private readonly IAdminService _adminService;

        public EditModel(IAdminService adminService)
        {
            _adminService = adminService;
        }

        public async Task<IActionResult> OnGetAsync(int id, int moduleId, int courseId)
        {
            try
            {
                Alert = String.Empty;
                ViewData["Modules"] = (await _adminService.GetAsync<Module, ModuleDTO>(include: m => m.Course))
                   .ToSelectList("Id", "CourseAndModule");

                Input = await _adminService.SingleAsync<Download, DownloadDTO>(
                    v => v.Id.Equals(id) && v.ModuleId.Equals(moduleId) && v.CourseId.Equals(courseId));

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
                var id = Input.ModuleId;                
                Input.CourseId = (await _adminService.SingleAsync<Module, ModuleDTO>(m => m.Id.Equals(id)))
                    .CourseId;
                var succeeded = await _adminService.UpdateAsync<DownloadDTO, Download>(Input);
                if (succeeded)
                {
                    Alert = $"Updated Download: {Input.Title}.";
                    return RedirectToPage("Index");
                }
            }
            ViewData["Modules"] = (await _adminService.GetAsync<Module, ModuleDTO>(include: m => m.Course))
                   .ToSelectList("Id", "CourseAndModule");
            return Page();
        }
    }
}