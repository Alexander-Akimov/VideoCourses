using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
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
    }
}
