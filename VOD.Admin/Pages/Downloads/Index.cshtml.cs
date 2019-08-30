using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VOD.Admin.Models;
using VOD.Domain.Interfaces;

using VOD.Domain.DTOModles.Admin;
using VOD.Domain.Entities;

namespace VOD.Admin.Pages.Downloads
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        public IEnumerable<DownloadDTO> Items { get; set; } = new List<DownloadDTO>();
        [TempData]
        public string Alert { get; set; }

        private readonly IAdminService _adminService;
        public IndexModel(IAdminService adminService)
        {
            _adminService = adminService;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                Items = await _adminService.GetAsync<Download, DownloadDTO>(m => m.Course, m => m.Module);
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