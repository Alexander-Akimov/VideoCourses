using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VOD.Domain.DTOModles;
using VOD.Domain.DTOModles.Admin;

namespace VOD.Domain.Interfaces
{
    public interface IHttpClientFactoryService
    {
        Task<List<TResponse>> GetListAsync<TResponse>(string uri, string serviceName, string token = "") where TResponse : class;

        Task<TResponse> GetAsync<TResponse>(string uri, string serviceName, string token = "") where TResponse : class;

        Task<TokenDTO> CreateTokenAsync(LoginUserDTO user, string uri, string serviceName, string token ="");

        Task<TokenDTO> GetTokenAsync(LoginUserDTO user, string uri, string serviceName, string token ="");
    }
}
