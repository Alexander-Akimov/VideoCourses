using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VOD.Admin.Models;
using VOD.Common.Services;
using VOD.Database.Services;
using VOD.Domain.DTOModles.Admin;
using VOD.Domain.Entities;

namespace VOD.Admin.Pages.Courses
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        public IEnumerable<CourseDTO> Items { get; set; } = new List<CourseDTO>();
        [TempData]
        public string Alert { get; set; }

        private readonly IAdminCoursesService _adminService;
        public IndexModel(IAdminCoursesService adminService)
        {
            _adminService = adminService;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                Items = await _adminService.GetAsync(true);
                return Page();
            }
            catch// (Exception)
            {
                Alert = "You do not have access to this page.";
                return RedirectToPage("/Index");
            }
        }
    }
}