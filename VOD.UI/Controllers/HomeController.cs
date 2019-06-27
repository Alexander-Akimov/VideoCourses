using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VOD.UI.Models;
using Microsoft.AspNetCore.Identity;
using VOD.Domain.Entities;
using VOD.Database.Services;

namespace VOD.UI.Controllers
{
    public class HomeController : Controller
    {
        private IUIReadService _uiReadService;
        private SignInManager<VODUser> _signInManager;
        public HomeController(SignInManager<VODUser> signInMgr, IUIReadService uiReadService)
        {
            _signInManager = signInMgr;
            _uiReadService = uiReadService;
        }

        public IActionResult Index()
        {
            // var courses = await _uiReadService.GetCoursesAsync("25c705c0-76c8-4296-89d2-2128deb96280");
            // var course = await _uiReadService.GetCourseAsync("25c705c0-76c8-4296-89d2-2128deb96280", 1);
            // var video = await _uiReadService.GetVideoAsync("25c705c0-76c8-4296-89d2-2128deb96280", 1);
            // var videos = await _uiReadService.GetVideosAsync("25c705c0-76c8-4296-89d2-2128deb96280", 1);

            if (!_signInManager.IsSignedIn(User))
                return RedirectToPage("/Account/Login", new { Area = "Identity" });

            return RedirectToAction("Dashboard", "Membership");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
