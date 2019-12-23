using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Net.Client;
using GrpcProtoLib.Protos;
using VOD.Common.Services;
using VOD.Domain.DTOModles;

namespace VOD.Grpc.Common.Services
{
    public class AdminGrpcClientService : IAdminGrpcClientService
    {
        private readonly IJwtTokenService _jwtTokenService;
        private TokenDTO _token = new TokenDTO();

        public AdminGrpcClientService(IJwtTokenService jwtTokenService)
        {
            _jwtTokenService = jwtTokenService;
        }

        public Task<bool> AnyAsync<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : class
        {
            throw new NotImplementedException();
        }

        private async Task<GrpcChannel> CreateAuthenticatedChannel(string address)
        {
            _token = await _jwtTokenService.CheckTokenAsync(_token);

            var credentials = CallCredentials.FromInterceptor((context, metadata) =>
            {
                if (!string.IsNullOrEmpty(_token.Token))
                {
                    metadata.Add("Authorization", $"Bearer {_token.Token}");
                }
                return Task.CompletedTask;
            });

            // SslCredentials is used here because this channel is using TLS.
            // CallCredentials can't be used with ChannelCredentials.Insecure on non-TLS channels.
            var channel = GrpcChannel.ForAddress(address, new GrpcChannelOptions
            {
                Credentials = ChannelCredentials.Create(new SslCredentials(), credentials)
            });
            return channel;
        }

        public async Task<int> CreateAsync<TSource, TDestination>(TSource item)
            where TSource : class
            where TDestination : class
        {
            _token = await _jwtTokenService.CheckTokenAsync(_token);

            var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new Courses.CoursesClient(channel);            

            var headers = new Metadata();
            headers.Add("Authorization", $"Bearer {_token.Token}");            

            var reply = await client.CreateCourseAsync(
                new CourseMessage { Title = "BestTitle" }, headers);

            /*  var channel = await CreateAuthenticatedChannel("https://localhost:5001");
              var client = new CoursesService.CoursesServiceClient(channel);
              */
            /* var serviceFactory = _grpcServiceFactory.GetService<TSource>();
             serviceFactory

             var reply = await client.CreateCourseAsync(
                new CourseMessage { Title = "BestTitle" });.*/

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
