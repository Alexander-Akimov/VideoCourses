using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VOD.Common.Constants;
using VOD.Common.Services;
using VOD.Domain.DTOModles;
using VOD.Domain.Entities;
using VOD.Domain.Interfaces;

namespace VOD.Domain.Services.Services
{
    public class AdminAPIService : IAdminService
    {
        private readonly IHttpClientFactoryService _http;
        private readonly IJwtTokenService _jwtTokenService;
        private Dictionary<string, object> _properties = new Dictionary<string, object>();
        private TokenDTO _token = new TokenDTO();

        public AdminAPIService(IHttpClientFactoryService http, IJwtTokenService jwtTokenService)
        {
            _http = http;
            _jwtTokenService = jwtTokenService;
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
                _token = await _jwtTokenService.CheckTokenAsync(_token);

                return await _http.GetListAsync<TDestination>($"{uri}?include={include.ToString()}", AppConstants.HttpClientName, _token?.Token);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Task<List<TDestination>> GetAsync<TSource, TDestination>(Expression<Func<TSource, bool>> expression, bool include = false)
            where TSource : class
            where TDestination : class
        {
            throw new NotImplementedException();
        }

        public async Task<TDestination> SingleAsync<TSource, TDestination>(Expression<Func<TSource, bool>> whereExpr, bool include = false)
            where TSource : class
            where TDestination : class
        {
            try
            {
                GetProperties(whereExpr);
                string uri = FormatUriWithIds<TSource>();
                _token = await _jwtTokenService.CheckTokenAsync(_token);

                return await _http.GetAsync<TDestination>($"{uri}?include={include.ToString()}", AppConstants.HttpClientName, _token.Token);
            }
            catch (Exception ex)
            {
                throw;
            }
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
        //GetProperties<Instructor>(instr => instr.Id == 3);
        //GetProperties<Instructor>(instr => instr.Id.Equals(3));
        //var id = 4;
        //Expression<Func<Instructor, bool>> expr = param => param.Id.Equals(id);
        private void GetProperties<TSource>(Expression<Func<TSource, bool>> expression)
        {
            try
            {
                var lambda = expression as LambdaExpression;

                _properties.Clear();
                ResolveExpression(lambda.Body);
                var typeName = typeof(TSource).Name;

                if (!typeName.Equals("Instructor") && !typeName.Equals("Course") && !_properties.ContainsKey("courseId"))
                    _properties.Add("courseId", 0);
            }
            catch { throw; }
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
        private string FormatUriWithIds<TSource>()
        {
            string uri = "api";
            object id, moduleId, courseId;
            bool succeded = _properties.TryGetValue("courseId", out courseId);
            if (succeded)
                uri = $"{uri}/courses/{courseId}";

            succeded = _properties.TryGetValue("moduleId", out moduleId);
            if (succeded)
                uri = $"{uri}/modules/{moduleId}";

            uri = $"{uri}/{typeof(TSource).Name}s";

            succeded = _properties.TryGetValue("id", out id);
            if (succeded)
                uri = $"{uri}/{id}";
            return uri;
        }

        private void ResolveExpression(Expression expression)
        {
            try
            {
                if (expression is BinaryExpression)
                {
                    if (expression.NodeType == ExpressionType.AndAlso)
                    {
                        var binaryExpression = expression as BinaryExpression;
                        ResolveExpression(binaryExpression.Left);
                        ResolveExpression(binaryExpression.Right);
                    }
                    else if (expression.NodeType == ExpressionType.Equal)
                    {
                        var binaryExpression = expression as BinaryExpression;
                        ResolveExpression(binaryExpression.Left);
                        ResolveExpression(binaryExpression.Right);
                    }
                }
                else if (expression is ConstantExpression)
                {
                    var value = ((ConstantExpression)expression).Value;
                    var tmp = _properties["temp"];
                    if (tmp != null)
                    {
                        _properties.Add(tmp.ToString(), value);
                        _properties.Remove("temp");
                    }
                    else
                    {
                        _properties.Add("temp", value);
                    }
                }
                else if (expression is MemberExpression)
                {
                    var memberExpression = expression as MemberExpression;
                    var tmp = _properties["temp"];
                    if (tmp != null)
                    {
                        _properties.Add(memberExpression.Member.Name, tmp);
                        _properties.Remove("temp");
                    }
                    else
                    {
                        _properties.Add("temp", memberExpression.Member.Name);
                    }

                }
                else if (expression is MethodCallExpression)
                {
                    GetExpressionProperties(expression);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
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
            else if (argument is ConstantExpression)
            {
                var memberExpression = body.Object as MemberExpression;
                var val = ((ConstantExpression)argument).Value;

                _properties.Add(memberExpression.Member.Name, val);

                /*  var body = expression as MethodCallExpression;
                  if (body.Object is MemberExpression)
                  {
                      var arg = body.Arguments[0];
                      var memberExpression = body.Object as MemberExpression;
                      var val = ((ConstantExpression)arg).Value;

                      _properties.Add(memberExpression.Member.Name, val);
                  }*/
            }

        }
    }
}
