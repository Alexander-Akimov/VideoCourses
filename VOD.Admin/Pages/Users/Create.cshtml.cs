using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using VOD.Domain.DTOModles.Admin;
using VOD.Domain.Interfaces;

namespace VOD.Admin.Pages.Users
{
    [Authorize(Roles = "Admin")]
    public class CreateModel : PageModel
    {
        [BindProperty]
        public RegisterUserDTO Input { get; set; } = new RegisterUserDTO();

        [TempData]
        public string Alert { get; set; }

        private readonly IUserService _userService;

        public CreateModel(IUserService userService)
        {
            _userService = userService;
        }
      /*  public async Task<IActionResult> OnGetAsync()
        {
            return Page();
        }*/
        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.AddUserAsync(Input);
                if (result.Succeeded)
                {
                    Alert = $"Created a new account for {Input.Email}.";
                    return RedirectToPage("Index");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return Page();
        }
    }
}