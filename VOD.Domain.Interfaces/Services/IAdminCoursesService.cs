using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VOD.Domain.DTOModles.Admin;

namespace VOD.Domain.Interfaces
{
    public interface IAdminCoursesService
    {
        Task<List<CourseDTO>> GetAsync(bool include = false);
        Task<CourseDTO> SingleAsync(int id, bool include = false);
        Task<Boolean> DeleteAsync(int id);
    }
}
