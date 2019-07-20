using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VOD.Database.Services;
using VOD.Domain.DTOModles.Admin;
using VOD.Domain.Entities;

namespace VOD.Admin.Pages.Users
{
    [Authorize(Roles = "Admin")]
    public class DetailsModel : PageModel
    {
        private readonly IDbWriteService _dbWriteService;
        private readonly IDbReadService _dbReadService;
        public IEnumerable<Course> Courses { get; set; } = new List<Course>();
        public SelectList AvailableCourses { get; set; }
        public UserDTO Customer { get; set; }

        [Display(Name = "Available Courses")]
        [BindProperty]
        public int CourseId { get; set; }

        public DetailsModel(IDbReadService dbReadService, IDbWriteService dbWriteService)
        {
            _dbReadService = dbReadService;
            _dbWriteService = dbWriteService;
        }
        public void OnGet()
        {

        }
        public async Task OnPostAsync()
        {
            try
            {

            }
            catch (Exception)
            {

                //throw;
            }

           // return Page();
        }

        private async Task FillViewData(string userId)
        {
            var user = await _dbReadService.SingleAsync<VODUser>(u => u.Id.Equals(userId));
            Customer = new UserDTO { Id = user.Id, Email = user.Email };

            var userCourses = await _dbReadService.GetQueryAsync<UserCourse>()//compact query
                .Where(uc => uc.UserId.Equals(userId))
                .Select(c => c.Course)
                .ToListAsync();
            var userCourseIds = userCourses.Select(uc => uc.Id);

            Courses = userCourses;

        }
    }
}