using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VOD.Domain.Interfaces;

namespace VOD.Domain.Services.Services
{
    public class AdminAPIService : IAdminService
    {
        private readonly IHttpClientFactoryService _http;

        public AdminAPIService(IHttpClientFactoryService http)
        {
            _http = http;
        }

        public Task<bool> AnyAsync<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : class
        {
            throw new NotImplementedException();
        }

        public Task<int> CreateAsync<TSourse, TDestination>(TSourse item)
            where TSourse : class
            where TDestination : class
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync<TSourse>(Expression<Func<TSourse, bool>> expression) where TSourse : class
        {
            throw new NotImplementedException();
        }

        public Task<List<TDestination>> GetAsync<TSourse, TDestination>(params Expression<Func<TSourse, object>>[] include)
            where TSourse : class
            where TDestination : class
        {
            throw new NotImplementedException();
        }

        public Task<List<TDestination>> GetAsync<TSourse, TDestination>(Expression<Func<TSourse, bool>> whereExpr, params Expression<Func<TSourse, object>>[] include)
            where TSourse : class
            where TDestination : class
        {
            throw new NotImplementedException();
        }

        public Task<List<TDestination>> GetAsync<TSourse, TDestination>(bool include = false)
            where TSourse : class
            where TDestination : class
        {
            throw new NotImplementedException();
        }

        public Task<List<TDestination>> GetAsync<TSourse, TDestination>(Expression<Func<TSourse, bool>> expression, bool include = false)
            where TSourse : class
            where TDestination : class
        {
            throw new NotImplementedException();
        }

        public Task<TDestination> SingleAsync<TSourse, TDestination>(Expression<Func<TSourse, bool>> whereExpr, bool include = false)
            where TSourse : class
            where TDestination : class
        {
            throw new NotImplementedException();
        }

        public Task<TDestination> SingleAsync<TSourse, TDestination>(Expression<Func<TSourse, bool>> whereExpr, params Expression<Func<TSourse, object>>[] include)
            where TSourse : class
            where TDestination : class
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync<TSourse, TDestination>(TSourse item)
            where TSourse : class
            where TDestination : class
        {
            throw new NotImplementedException();
        }
    }
}
