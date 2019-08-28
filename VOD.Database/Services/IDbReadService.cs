using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Linq;

namespace VOD.Database.Services
{
    public interface IDbReadService
    {
        Task<TEntity> SingleAsync<TEntity>(Expression<Func<TEntity, bool>> expression = null,
            params Expression<Func<TEntity, object>>[] includeProps) where TEntity : class;
        IQueryable<TEntity> GetQuery<TEntity>(Expression<Func<TEntity, bool>> expression = null, bool include = false) where TEntity : class;
        Task<List<TEntity>> GetAsync<TEntity>(params Expression<Func<TEntity, object>>[] includeProps) where TEntity : class;
        Task<List<TEntity>> GetAsync<TEntity>(Expression<Func<TEntity, bool>> expression = null, bool include = false) where TEntity : class;
        Task<TEntity> SingleAsync<TEntity>(Expression<Func<TEntity, bool>> expression = null, bool include = false) where TEntity : class;
        Task<bool> AnyAsync<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : class;
        void Include<TEntity>() where TEntity : class;
        void Include<TEntity1, TEntity2>() where TEntity1 : class where TEntity2 : class;
        (int courses, int downloads, int instructors, int modules, int videos, int users) Count();
        Task<List<TEntity>> GetAsync<TEntity>(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, object>>[] includeProps) where TEntity : class;
    }
}
