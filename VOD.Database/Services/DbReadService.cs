using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace VOD.Database.Services
{

    public class DbReadService : IDbReadService
    {
        private VODContext _dBContext;
        public DbReadService(VODContext db)
        {
            _dBContext = db;
        }

        public async Task<bool> AnyAsync<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : class
        {
            return await _dBContext.Set<TEntity>().AnyAsync(expression);
        }

        public IQueryable<TEntity> GetQuery<TEntity>(Expression<Func<TEntity, bool>> expression = null, bool include = false) where TEntity : class
        {
            var items = _dBContext.Set<TEntity>().AsQueryable();
            if (include)
                items = Include(items);

            if (expression != null)
                items = items.Where(expression);

            return items;
        }
        public async Task<List<TEntity>> GetAsync<TEntity>(params Expression<Func<TEntity, object>>[] includeProps) where TEntity : class
        {
            var items = _dBContext.Set<TEntity>().AsQueryable();

            foreach (var prop in includeProps)
                items = items.Include(prop);

            return await items.ToListAsync();
        }

        public async Task<List<TEntity>> GetAsync<TEntity>(Expression<Func<TEntity, bool>> expression = null, bool include = false) where TEntity : class
        {
            var items = _dBContext.Set<TEntity>().AsQueryable();
            if (include)
                items = Include(items);

            if (expression != null)
                items = items.Where(expression);

            //var items2 = from item in items
            //            // where expression
            //            select item;
            return await items.ToListAsync();
        }

        //public async Task<List<TEntity>> GetAsync<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : class
        //{
        //    return await _db.Set<TEntity>().Where(expression).ToListAsync();
        //}

        public async Task<TEntity> SingleAsync<TEntity>(Expression<Func<TEntity, bool>> expression = null, bool include = false) where TEntity : class
        {
            var items = _dBContext.Set<TEntity>().AsQueryable();
            if (include)
                items = Include(items);

            if (expression != null)
                items = items.Where(expression);

            return await items.SingleOrDefaultAsync();
        }

        public void Include<TEntity>() where TEntity : class
        {
            var propertyNames = _dBContext.Model.FindEntityType(typeof(TEntity))
                .GetNavigations()
                .Select(e => e.Name);

            foreach (var name in propertyNames)
                _dBContext.Set<TEntity>().Include(name).Load();
        }

        public void Include<TEntity1, TEntity2>()
            where TEntity1 : class
            where TEntity2 : class
        {
            Include<TEntity1>();
            Include<TEntity2>();
        }

        private IQueryable<TEntity> Include<TEntity>(IQueryable<TEntity> items) where TEntity : class
        {
            var props = GetNavPropsNames<TEntity>();
            foreach (var prop in props)
                items = items.Include(prop);
            return items;
            //return items.Include(props);
        }
        private IEnumerable<string> GetNavPropsNames<TEntity>()
        {
            return _dBContext.Model.FindEntityType(typeof(TEntity))
                 .GetNavigations()
                 .Select(e => e.Name);
        }

        public (int courses, int downloads, int instructors, int modules, int videos, int users) Count()
        {
            return (
                 courses: _dBContext.Courses.Count(),
                 downloads: _dBContext.Downloads.Count(),
                 instructors: _dBContext.Instructors.Count(),
                 modules: _dBContext.Modules.Count(),
                 videos: _dBContext.Videos.Count(),
                 users: _dBContext.Users.Count());
        }
    }
}
