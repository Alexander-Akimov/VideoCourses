using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VOD.Domain.DTOModles.Admin;
using VOD.Domain.Entities;

namespace VOD.Database.Services
{
    public class AdminCoursesService : IAdminCoursesService
    {
        private readonly IDbReadService _dbReadService;
        private readonly IDbWriteService _dbWriteService;
        private readonly IMapper _mapper;
        public AdminCoursesService(
            IDbReadService dbReadService,
            IDbWriteService dbWriteService,
            IMapper mapper)
        {
            _dbReadService = dbReadService;
            _dbWriteService = dbWriteService;
            _mapper = mapper;
        }
        public async Task<List<CourseDTO>> GetAsync(bool include = false)
        {
            //if (include)               
            //var entities = await _dbReadService.GetAsync<TSourse>();
            var courses = await _dbReadService.GetQuery<Course>()
                .Include(c => c.Instructor)
                .ToListAsync();

            return _mapper.Map<List<CourseDTO>>(courses);
        }

        public async Task<CourseDTO> SingleAsync(int id, bool include = false)
        {
            var course = await _dbReadService.GetQuery<Course>()
                .Where(c => c.Id.Equals(id))
                .Include(c => c.Instructor)
                .SingleOrDefaultAsync();

            return _mapper.Map<CourseDTO>(course);
        }

        public async Task<Boolean> DeleteAsync(int id)
        {
            try
            {
                //TODO: TWO Queries to delete entity????
                //_dbWriteService.Delete(new Entity{Id = })
                var entity = await _dbReadService.SingleAsync<Course>(d => d.Equals(id));
                _dbWriteService.Delete(entity);

                return await _dbWriteService.SaveChangesAsync();
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
