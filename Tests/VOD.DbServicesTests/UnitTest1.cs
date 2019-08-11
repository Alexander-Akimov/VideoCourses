using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using VOD.Database.Services;
using VOD.Domain.Entities;
using Xunit;

namespace VOD.DbServicesTests
{
    public class UnitTest1
    {
        private IDbReadService _dbReadService;
        private IUIReadService _uiReadService;
        [Fact]
        public async void Test1()
        {
            //_db.Include<Download>();
            //_db.Include<Module, Course>();

            var result1 = await _dbReadService.SingleAsync<Download>(d => d.Id.Equals(3));

            var result2 = await _dbReadService.GetAsync<Download>(include: true); // Fetch all
                                                                                  // Fetch all that matches the Lambda expression
            var result3 = await _dbReadService.GetAsync<Download>(d => d.ModuleId.Equals(1));

            var result4 = await _dbReadService.AnyAsync<Download>(d => d.ModuleId.Equals(1)); // True if a record is found


        }
        [Fact]
        public async void GetCourseAsync()
        {
            /*     var userCourse = await _db.SingleAsync<UserCourse>(
                           expression: uc => uc.UserId.Equals(userId) && uc.CourseId.Equals(courseId));

                       if (userCourse == null) return default;

                       var course = await _db.SingleAsync<Course>(c => c.Id.Equals(courseId),
                            include: true);      */

            var courseId = 1;
            await _dbReadService.GetQuery<Course>(c => c.Id.Equals(courseId))
                          .Select(c => c.Instructor).LoadAsync(); // load course's instructor
        }

        [Fact]
        public async void GetCoursesDbReadService()
        {
            var userId = "25c705c0-76c8-4296-89d2-2128deb96280";
            var courses = await _dbReadService.GetAsync<UserCourse>(//bad query
                expression: uc => uc.UserId.Equals(userId),
                include: true);

            // courses.Select(uc => uc.Course).ToList(); //this commented because include: true

            var courses2 = _dbReadService.GetQuery<UserCourse>()//compact query
               .Where(uc => uc.UserId.Equals(userId))
               .Select(c => c.Course);
        }


        [Fact]
        public async void GetVideosLinq()
        {
            string userId = "25c705c0-76c8-4296-89d2-2128deb96280";
            int moduleId = 1;

            var videosQuery = _dbReadService.GetQuery<UserCourse>()
                .Join(_dbReadService.GetQuery<Course>(),
                    uc => uc.CourseId,
                    c => c.Id,
                    (uc, c) => new { c.Id, uc.UserId }
                )
                .Join(_dbReadService.GetQuery<Video>(),
                    ce => ce.Id,
                    video => video.CourseId,
                    (ce, video) => new { ce, video }
                ).Where(c => c.video.ModuleId.Equals(moduleId) && c.ce.UserId.Equals(userId))
                .Select(comb => comb.video);

            await videosQuery.ToListAsync();
        }

        [Fact]
        public async void GetVideosLinq2()
        {
            string userId = "25c705c0-76c8-4296-89d2-2128deb96280";
            int moduleId = 1;

            var videosQuery = from v in _dbReadService.GetQuery<Video>()
                              join c in _dbReadService.GetQuery<Course>() on v.CourseId equals c.Id
                              join uc in _dbReadService.GetQuery<UserCourse>() on c.Id equals uc.CourseId
                              where v.ModuleId == moduleId && uc.UserId == userId
                              select v;

            await videosQuery.ToListAsync();
        }

        [Fact]
        public async void GetVideosLinq3()
        {
            string userId = "25c705c0-76c8-4296-89d2-2128deb96280";
            int moduleId = 1;
            var module = await _dbReadService.SingleAsync<Module>(m => m.Id.Equals(moduleId));
            if (module == null) return;

            var userCourse = await _dbReadService.SingleAsync<UserCourse>(
               c => c.UserId.Equals(userId) && c.CourseId.Equals(module.CourseId));
            if (userCourse == null) return;

            var videos = await _dbReadService.GetQuery<Video>()
                 .Where(v => v.ModuleId == module.Id)
                 .ToListAsync();
        }

        [Fact]
        public async void GetCourses()
        {
            var courses = await _uiReadService.GetCoursesAsync("25c705c0-76c8-4296-89d2-2128deb96280");
            courses.ToList();

            var course = await _uiReadService.GetCourseAsync("25c705c0-76c8-4296-89d2-2128deb96280", 1);
        }
    }
}
