using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VOD.Domain.Entities;

namespace VOD.Domain.Interfaces
{
    public interface IUIReadService
    {
        Task<IEnumerable<Course>> GetCoursesAsync(string userId);
        Task<Course> GetCourseAsync(string userId, int courseId);
        Task<Video> GetVideoAsync(string userId, int videoId);
        Task<IEnumerable<Video>> GetVideosAsync(string userId, int moduleId = default(int));
    }
}
