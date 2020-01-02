using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Net.Client;
using VOD.Domain.Interfaces.Services;

namespace VOD.Grpc.Common.Services
{
    public class GrpcServiceFactory
    {
       /* private readonly Dictionary<Type, Func<IGenericAdminService>> _map =
            new Dictionary<Type, Func<IGenericAdminService>>();

        public GrpcServiceFactory()
        {

        }

        public IGenericAdminService GetService<EntityType>() where EntityType : class
        {
            EntityType entityType;

            
        }
        */
        /* public ClientBase<TServiceClient> GetService<TServiceClient>(Type serviceClientType)
              where TServiceClient : class
          {
              if(serviceClientType is CoursesServiceClient)
              {

              }

          }*/
       /* private ClientBase GetServiceByType(Type dataType)
        {
            /*if(dataType is )
            {

            }
            var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new CoursesService.CoursesServiceClient(channel);
            return client;
        }

        public async Task<int> CreateAsync<TSource, TDestination>(TSource item)
           where TSource : class
           where TDestination : class
        {

        }*/
    }
}
