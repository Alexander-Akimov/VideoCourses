using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VOD.Domain.Entities;
using VOD.Domain.Interfaces;

namespace VOD.Domain.Services
{
    public class UIReadService : IUIReadService
    {
        private readonly IDbReadService _dbReadService;
        public UIReadService(IDbReadService dbReadService)
        {
            _dbReadService = dbReadService;
        }

        public async Task<Course> GetCourseAsync(string userId, int courseId) // produces two simple sql queries
        {
            await _dbReadService.GetQuery<Module>() // load course's modules
                .Where(c => c.CourseId.Equals(courseId))
                .LoadAsync();

            await _dbReadService.GetQuery<Video>() // load course's videos
                .Where(v => v.CourseId.Equals(courseId))
                .LoadAsync();

            await _dbReadService.GetQuery<Download>() // load course's Downloads
                .Where(v => v.CourseId.Equals(courseId))
                .LoadAsync();

            var userCourse = await _dbReadService.GetQuery<UserCourse>() // one row sql query result
                .Where(uc => uc.UserId.Equals(userId) && uc.CourseId.Equals(courseId))
                .Include(c => c.Course) // load course
                .ThenInclude(c => c.Instructor)  // load course's instructor
                .FirstOrDefaultAsync();

            return userCourse?.Course ?? default;
        }

        public async Task<IEnumerable<Course>> GetCoursesAsync(string userId)
        {
            // var userId = "25c705c0-76c8-4296-89d2-2128deb96280";
            // var courses1 = await _dbReadService.GetAsync<UserCourse>(uc => uc.UserId.Equals(userId), true);

            var courses = _dbReadService.GetQuery<UserCourse>()
                .Where(uc => uc.UserId.Equals(userId))
                .Select(c => c.Course);

            return await courses.ToListAsync();
        }

        public async Task<Video> GetVideoAsync(string userId, int videoId)
        {
            //пользователь не может получить видео тех курсов которых у него нет
            var video = await _dbReadService.SingleAsync<Video>(v => v.Id.Equals(videoId), true);
            if (video == null) return default;

            var userCourse = await _dbReadService.SingleAsync<UserCourse>(
                c => c.UserId.Equals(userId) && c.CourseId.Equals(video.CourseId));
            if (userCourse == null) return default;

            return video;
        }

        public async Task<IEnumerable<Video>> GetVideosAsync(string userId, int moduleId = 0)
        {
            var module = await _dbReadService.SingleAsync<Module>(m => m.Id.Equals(moduleId));
            if (module == null) return default;

            var userCourse = await _dbReadService.SingleAsync<UserCourse>(
               c => c.UserId.Equals(userId) && c.CourseId.Equals(module.CourseId));
            if (userCourse == null) return default;

            var videos = await _dbReadService.GetQuery<Video>()
                 .Where(v => v.ModuleId == module.Id)
                 .ToListAsync();

            //var videos2 = await _dbReadService.GetQueryAsync<Video>()
            //    .Where(v => v.Module.Id == moduleId && v.CourseId == userCourse.CourseId)
            //    .ToListAsync();

            return videos;
        }
    }
}
