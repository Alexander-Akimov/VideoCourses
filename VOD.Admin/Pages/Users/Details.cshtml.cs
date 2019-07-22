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
using VOD.Common.Extensions;

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

        [BindProperty, Display(Name = "Available Courses")]
        public int CourseId { get; set; }

        public DetailsModel(IDbReadService dbReadService, IDbWriteService dbWriteService)
        {
            _dbReadService = dbReadService;
            _dbWriteService = dbWriteService;
        }
        public async Task OnGetAsync(string id)
        {
            await FillViewDataAsync(userId: id);
        }
        public async Task<IActionResult> OnPostAddAsync(string userId)
        {
            try
            {
                _dbWriteService.Add(new UserCourse { CourseId = CourseId, UserId = userId });

                var succeeded = await _dbWriteService.SaveChangesAsync();
            }
            catch (Exception)
            {
            }
            await FillViewDataAsync(userId);
            return Page();
        }
        public async Task<IActionResult> OnPostRemoveAsync(int courseId, string userId)
        {
            try
            {
                var userCourse = await _dbReadService.SingleAsync<UserCourse>(
                    uc => uc.UserId.Equals(userId) &&
                    uc.CourseId.Equals(courseId));

                if (userCourse != null)
                {
                    _dbWriteService.Delete(userCourse);
                    await _dbWriteService.SaveChangesAsync();
                }
            }
            catch (Exception)
            {
                //throw;
            }
            await FillViewDataAsync(userId);
            return Page();
        }

        private async Task FillViewDataAsync(string userId)
        {
            var user = await _dbReadService.SingleAsync<VODUser>(u => u.Id.Equals(userId));
            Customer = new UserDTO { Id = user.Id, Email = user.Email };

            Courses = await _dbReadService.GetQueryAsync<UserCourse>()//produces compact sql query
                .Where(uc => uc.UserId.Equals(userId))
                .Select(uc => uc.Course)
                .ToListAsync();

            var userCourseIds = Courses.Select(uc => uc.Id);

            var availableCourses = await _dbReadService.GetAsync<Course>(
                c => !userCourseIds.Contains(c.Id));

            AvailableCourses = availableCourses.ToSelectList("Id", "Title");
        }
    }
}