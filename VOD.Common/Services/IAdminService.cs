using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace VOD.Common.Services
{
    public interface IAdminService
    {
        Task<List<TDestination>> GetAsync<TSourse, TDestination>(
            params Expression<Func<TSourse, object>>[] include) where TSourse : class where TDestination : class;
        Task<List<TDestination>> GetAsync<TSourse, TDestination>(bool include = false) where TSourse : class where TDestination : class;
        Task<List<TDestination>> GetAsync<TSourse, TDestination>(Expression<Func<TSourse, bool>> expression, bool include = false) where TSourse : class where TDestination : class;
        Task<TDestination> SingleAsync<TSourse, TDestination>(Expression<Func<TSourse, bool>> expression, bool include = false) where TSourse : class where TDestination : class;
        Task<int> CreateAsync<TSourse, TDestination>(TSourse item) where TSourse : class where TDestination : class;
        Task<bool> UpdateAsync<TSourse, TDestination>(TSourse item) where TSourse : class where TDestination : class;
        Task<bool> DeleteAsync<TSourse>(Expression<Func<TSourse, bool>> expression) where TSourse : class;
        Task<bool> AnyAsync<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : class;
    }
}
