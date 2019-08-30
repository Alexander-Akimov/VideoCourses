using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VOD.Admin.Models;

using VOD.Domain.DTOModles.Admin;
using VOD.Domain.Interfaces;

namespace VOD.Admin.Pages.Users
{
    [Authorize(Roles ="Admin")]
    public class IndexModel : PageModel
    {
        public IEnumerable<UserDTO> Users { get; set; } = new List<UserDTO>();
        [TempData]
        public string Alert { get; set; }

        private readonly IUserService _userService;
        public IndexModel(IUserService userService)
        {
            _userService = userService;
        }
        public async Task OnGetAsync()
        {
            Users = await _userService.GetUsersAsync();
        }
    }
}