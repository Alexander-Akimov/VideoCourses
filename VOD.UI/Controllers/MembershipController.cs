using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VOD.Database;
using VOD.Database.Services;
using VOD.Domain.DTOModles.UI;
using VOD.Domain.Entities;
using VOD.UI.Models.MembershipViewModels;

namespace VOD.UI.Controllers
{
    public class MembershipController : Controller
    {
        private readonly string _userId;
        public readonly IMapper _mapper;
        private readonly IUIReadService _uiReadService;

        public MembershipController(IHttpContextAccessor httpContextAccessor,
            UserManager<VODUser> userManager, IMapper mapper, IUIReadService uiReadService)
        {
            var user = httpContextAccessor.HttpContext.User;
            _userId = userManager.GetUserId(user);
            _mapper = mapper;
            _uiReadService = uiReadService;
        }

        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            var coursesDtoObjects = _mapper.Map<List<CourseDTO>>(
                await _uiReadService.GetCoursesAsync(_userId));

            var dashboardModel = new DashboardViewModel { Courses = new List<List<CourseDTO>>() };

            var rowsCount = coursesDtoObjects.Count <= 3 ? 1 : coursesDtoObjects.Count / 3;
            for (int i = 0; i < rowsCount; i++)
            {
                dashboardModel.Courses.Add(
                   coursesDtoObjects.Skip(i * 3).Take(3).ToList());
            }
            return View(dashboardModel);
        }

        [HttpGet]
        public async Task<IActionResult> Course(int id)
        {
            var course = await _uiReadService.GetCourseAsync(_userId, courseId: id);
            var mappedCourseDTO = _mapper.Map<CourseDTO>(course);
            var mappedInstructorDTO = _mapper.Map<InstructorDTO>(course.Instructor);
            var mappedModulesDTO = _mapper.Map<List<ModuleDTO>>(course.Modules);

            var courseModel = new CourseViewModel
            {
                Course = mappedCourseDTO,
                Instructor = mappedInstructorDTO,
                Modules = mappedModulesDTO
            };

            return View(courseModel);
        }

        [HttpGet]
        public async Task<IActionResult> Video(int id)
        {
            var video = await _uiReadService.GetVideoAsync(_userId, videoId: id);
            var course = await _uiReadService.GetCourseAsync(_userId, video.CourseId);

            var videoDto = _mapper.Map<VideoDTO>(video);
            var courseDto = _mapper.Map<CourseDTO>(course);
            var instructorDto = _mapper.Map<InstructorDTO>(course.Instructor);

            var videos = (await _uiReadService.GetVideosAsync(_userId, video.ModuleId))
                .OrderBy(o => o.Id).ToList();

            var count = videos.Count();
            var index = videos.FindIndex(v => v.Id.Equals(id));
            var previous = videos.ElementAtOrDefault(index - 1);
            var previousId = previous?.Id ?? 0;

            var next = videos.ElementAtOrDefault(index + 1);
            var nextId = next?.Id ?? 0;
            var nextTitle = next?.Title ?? string.Empty;
            var nextThumb = next?.Thumbnail ?? string.Empty;

            var videoViewModel = new VideoViewModel
            {
                Video = videoDto,
                Course = courseDto,
                Instructor = instructorDto,
                LessonInfo = new LessonInfoDTO
                {
                    LessonNumber = index + 1,
                    NumberOfLessons = count,
                    NextVideoId = nextId,
                    NextVideoTitle = nextTitle,
                    NextVideoThumbnail = nextThumb,
                    PreviousVideoId = previousId,
                    CurrentVideoTitle = videoDto.Title,
                    CurrentVideoThumbnail = videoDto.Thumbnail
                }
            };

            return View(videoViewModel);
        }
    }
}