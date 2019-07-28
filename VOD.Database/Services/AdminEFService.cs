using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VOD.Common.Services;

namespace VOD.Database.Services
{
    public class AdminEFService : IAdminService
    {
        public async Task<bool> AnyAsync<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : class
        {
            throw new NotImplementedException();
        }

        public async Task<int> CreateAsync<TSourse, TDestination>(TSourse item)
            where TSourse : class
            where TDestination : class
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteAsync<TSourse>(Expression<Func<TSourse, bool>> expression) where TSourse : class
        {
            throw new NotImplementedException();
        }

        public async Task<List<TDestination>> GetAsync<TSourse, TDestination>(bool include = false)
            where TSourse : class
            where TDestination : class
        {
            throw new NotImplementedException();
        }

        public async Task<List<TDestination>> GetAsync<TSourse, TDestination>(Expression<Func<TSourse, bool>> expression, bool include = false)
            where TSourse : class
            where TDestination : class
        {
            throw new NotImplementedException();
        }

        public async Task<TDestination> SingleAsync<TSourse, TDestination>(Expression<Func<TSourse, bool>> expression, bool include = false)
            where TSourse : class
            where TDestination : class
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateAsync<TSourse, TDestination>(TSourse item)
            where TSourse : class
            where TDestination : class
        {
            throw new NotImplementedException();
        }
    }
}
