using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace VOD.Domain.Interfaces
{
    public interface IHttpClientFactoryService
    {
        Task<List<TResponse>> GetListAsync<TResponse>(string uri, string serviceName, string token = "") where TResponse : class;
    }
}
