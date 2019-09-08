using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VOD.Common.Constants;
using VOD.Domain.Interfaces;

namespace VOD.Domain.Services.Services
{
    public class AdminAPIService : IAdminService
    {
        private readonly IHttpClientFactoryService _http;
        private Dictionary<string, object> _properties = new Dictionary<string, object>();

        public AdminAPIService(IHttpClientFactoryService http)
        {
            _http = http;
        }

        public Task<bool> AnyAsync<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : class
        {
            throw new NotImplementedException();
        }

        public Task<int> CreateAsync<TSource, TDestination>(TSource item)
            where TSource : class
            where TDestination : class
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync<TSource>(Expression<Func<TSource, bool>> expression) where TSource : class
        {
            throw new NotImplementedException();
        }

        public Task<List<TDestination>> GetAsync<TSource, TDestination>(params Expression<Func<TSource, object>>[] include)
            where TSource : class
            where TDestination : class
        {
            throw new NotImplementedException();
        }

        public Task<List<TDestination>> GetAsync<TSource, TDestination>(Expression<Func<TSource, bool>> whereExpr, params Expression<Func<TSource, object>>[] include)
            where TSource : class
            where TDestination : class
        {
            throw new NotImplementedException();
        }

        public async Task<List<TDestination>> GetAsync<TSource, TDestination>(bool include = false)
            where TSource : class
            where TDestination : class
        {
            try
            {
                GetProperties<TSource>();
                string uri = FormatUriWithoutIds<TSource>();

                return await _http.GetListAsync<TDestination>($"{uri}?include={include.ToString()}", AppConstants.HttpClientName);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<List<TDestination>> GetAsync<TSource, TDestination>(Expression<Func<TSource, bool>> expression, bool include = false)
            where TSource : class
            where TDestination : class
        {
            throw new NotImplementedException();
        }

        public Task<TDestination> SingleAsync<TSource, TDestination>(Expression<Func<TSource, bool>> whereExpr, bool include = false)
            where TSource : class
            where TDestination : class
        {


            throw new NotImplementedException();
        }

        public Task<TDestination> SingleAsync<TSource, TDestination>(Expression<Func<TSource, bool>> whereExpr, params Expression<Func<TSource, object>>[] include)
            where TSource : class
            where TDestination : class
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync<TSource, TDestination>(TSource item)
            where TSource : class
            where TDestination : class
        {
            throw new NotImplementedException();
        }

        private void GetProperties<TSource>()
        {
            _properties.Clear();
            var type = typeof(TSource);
            var id = type.GetProperty("Id");
            var moduleId = type.GetProperty("ModuleId");
            var courseId = type.GetProperty("CourseId");

            if (id != null)
                _properties.Add("id", 0);
            if (moduleId != null)
                _properties.Add("moduleId", 0);
            if (courseId != null)
                _properties.Add("courseId", 0);
        }

        private string FormatUriWithoutIds<TSource>()
        {
            string uri = "api";
            object moduleId, courseId;

            bool succeded = _properties.TryGetValue("courseId", out courseId);
            if (succeded)
                uri = $"{uri}/courses/0";

            succeded = _properties.TryGetValue("moduleId", out moduleId);
            if (succeded)
                uri = $"{uri}/modules/0";

            uri = $"{uri}/{typeof(TSource).Name}s"; //$"{nameof(TSource)}s";
            return uri;
        }

        private void GetExpressionProperties(Expression expression)
        {
            var body = (MethodCallExpression)expression;
            var argument = body.Arguments[0];
            if (argument is MemberExpression)
            {
                var memberExpression = argument as MemberExpression;
                var value = ((FieldInfo)memberExpression.Member).GetValue(
                    ((ConstantExpression)memberExpression.Expression).Value);

                _properties.Add(memberExpression.Member.Name, value);
            }
        }
    }
}
