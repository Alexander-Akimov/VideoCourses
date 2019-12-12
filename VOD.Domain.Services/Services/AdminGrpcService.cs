using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Net.Client;
using GrpcGreeter.Protos;
using VOD.Common.Services;
using VOD.Domain.DTOModles;
using VOD.Domain.Interfaces;
using VOD.Domain.Interfaces.Services;

namespace VOD.Domain.Services.Services
{
    public class AdminGrpcService : IAdminGrpcService
    {
        private readonly IJwtTokenService _jwtTokenService;
        private TokenDTO _token = new TokenDTO();

        public AdminGrpcService(IJwtTokenService jwtTokenService)
        {
            _jwtTokenService = jwtTokenService;
        }

        public Task<bool> AnyAsync<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : class
        {
            throw new NotImplementedException();
        }

        public async Task<int> CreateAsync<TSource, TDestination>(TSource item)
            where TSource : class
            where TDestination : class
        {
            _token = await _jwtTokenService.CheckTokenAsync(_token);

            var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var headers = new Metadata();
            headers.Add("Authorization", $"Bearer {_token.Token}");

            var client = new CoursesService.CoursesServiceClient(channel);

            var reply = await client.CreateCourseAsync(
                new CourseMessage { Title = "BestTitle" }, headers);

            return reply.Id;

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

        public Task<List<TDestination>> GetAsync<TSource, TDestination>(bool include = false)
            where TSource : class
            where TDestination : class
        {
            throw new NotImplementedException();
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
    }
}
