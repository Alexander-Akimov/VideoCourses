using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VOD.Common.Exceptions;
using VOD.Common.Extensions;
using VOD.Domain.Interfaces;

namespace VOD.Domain.Services
{
    public class HttpClientFactoryService : IHttpClientFactoryService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private CancellationTokenSource _cancellationTokenSource;
        private readonly CancellationToken _cancellationToken;

        public HttpClientFactoryService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _cancellationTokenSource = new CancellationTokenSource();
            _cancellationToken = _cancellationTokenSource.Token;
        }

        public async Task<List<TResponse>> GetListAsync<TResponse>(string uri, string serviceName, string token = "") where TResponse : class
        {
            try
            {
                if (new string[] { uri, serviceName }.IsNullOrEmptyOrWhiteSpace())
                    throw new HttpResponseException(HttpStatusCode.NotFound, "Could not find the resource");

                var httpClient = _httpClientFactory.CreateClient(serviceName);

                return await httpClient.GetListAsync<TResponse>(uri, _cancellationToken, token);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
