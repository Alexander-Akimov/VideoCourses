using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using VOD.Admin.Models;
using VOD.Database.Services;
using VOD.Domain.Entities;

namespace VOD.Admin.Pages
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        public (CardViewModel Instructors, CardViewModel Users,
            CardViewModel Courses, CardViewModel Modules,
            CardViewModel Videos, CardViewModel Downloads) Cards;
        [TempData]
        public string Alert { get; set; }

        private readonly IDbReadService _dbReadService;
        public IndexModel(IDbReadService dbReadService)
        {
            _dbReadService = dbReadService;
        }

        public void OnGet()
        {
            var (courses, downloads, instructors, modules, videos, users) = _dbReadService.Count();

            Cards = (
                Instructors: new CardViewModel
                {
                    BackgroundColor = "#9c27b0",
                    Count = instructors,
                    Description = "Instructors",
                    Icon = "person",
                    Url = "./Instructors/Index"
                },
                Users: new CardViewModel
                {
                    BackgroundColor = "#414141",
                    Count = users,
                    Description = "Users",
                    Icon = "people",
                    Url = "./Users/Index"
                },
                Courses: new CardViewModel
                {
                    BackgroundColor = "#009688",
                    Count = courses,
                    Description = "Courses",
                    Icon = "subscriptions",
                    Url = "./Courses/Index"
                },
                Modules: new CardViewModel
                {
                    BackgroundColor = "#f44336",
                    Count = modules,
                    Description = "Modules",
                    Icon = "list",
                    Url = "./Modules/Index"
                },
                Videos: new CardViewModel
                {
                    BackgroundColor = "#3f51b5",
                    Count = videos,
                    Description = "Videos",
                    Icon = "theaters",
                    Url = "./Videos/Index"
                },
                Downloads: new CardViewModel
                {
                    BackgroundColor = "#ffcc00",
                    Count = downloads,
                    Description = "Downloads",
                    Icon = "import_contacts",
                    Url = "./Downloads/Index"
                });
        }
    }
}
