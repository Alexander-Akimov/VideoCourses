﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VOD.Common.Exceptions;

namespace VOD.Common.Extensions
{
    public static class HttpClientExtensions
    {
        private static HttpRequestMessage CreateRequestHeaders(this string uri, HttpMethod httpMethod, string token = "")
        {
            var requestMessage = new HttpRequestMessage(httpMethod, uri);

            requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            if (!token.IsNullOrEmptyOrWhiteSpace())
                requestMessage.Headers.Authorization = new AuthenticationHeaderValue("bearer", token);

            if (httpMethod.Equals(HttpMethod.Get))
                requestMessage.Headers.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));

            return requestMessage;
        }

        private static async Task<StreamContent> SerializeRequestContentAsync<TRequest>(this TRequest content)
        {
            var memoryStream = new MemoryStream();

            await memoryStream.SerializeToJsonAndWriteAsync(content, new UTF8Encoding(), 1024, true);
            memoryStream.Seek(0, SeekOrigin.Begin);

            return new StreamContent(memoryStream);
        }

        private static async Task<HttpRequestMessage> CreateRequestContent<TRequest>(this HttpRequestMessage requestMessage, TRequest content)
        {
            requestMessage.Content = await content.SerializeRequestContentAsync();
            requestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return requestMessage;
        }


        private static async Task<TResponse> DeserializeResponse<TResponse>(this HttpResponseMessage response)
        {
            response.EnsureSuccessStatusCode();
            var responseStream = await response.Content.ReadAsStreamAsync();
            return responseStream.ReadAndDeserializeFromJson<TResponse>();
        }

        private static async Task CheckStatusCodes(this HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                object validationErrors = null;
                string message = string.Empty;
                if (response.StatusCode == (HttpStatusCode)422)
                {
                    var errorStream = await response.Content.ReadAsStreamAsync();
                    validationErrors = errorStream.ReadAndDeserializeFromJson();
                    message = "Could not process the entity";
                }
                else if (response.StatusCode == HttpStatusCode.BadRequest)
                    message = "Bad request.";
                else if (response.StatusCode == HttpStatusCode.Forbidden)
                    message = "Access denied.";
                else if (response.StatusCode == HttpStatusCode.NotFound)
                    message = "Could not find the entity.";
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                    message = "Not Logged In.";

                throw new HttpResponseException(response.StatusCode, message, validationErrors);
            }
            else
                response.EnsureSuccessStatusCode();
        }

        public static async Task<List<TResponse>> GetListAsync<TResponse>(this HttpClient httpClient,
            string uri, CancellationToken cancellationToken, string token = "")
        {
            try
            {
                var requestMessage = uri.CreateRequestHeaders(HttpMethod.Get, token);

                using (var response = await httpClient.SendAsync(requestMessage,
                    HttpCompletionOption.ResponseHeadersRead, cancellationToken))
                {
                    var stream = await response.Content.ReadAsStreamAsync();
                    await response.CheckStatusCodes();

                    return stream.ReadAndDeserializeFromJson<List<TResponse>>();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static async Task<TResponse> GetAsync<TRequest, TResponse>(this HttpClient httpClient,
           string uri, CancellationToken cancellationToken, TRequest content, string token = "")
        {
            try
            {
                var requestMessage = uri.CreateRequestHeaders(HttpMethod.Get, token);

                if (content != null)
                    await requestMessage.CreateRequestContent(content);


                using (var response = await httpClient.SendAsync(requestMessage,
                    HttpCompletionOption.ResponseHeadersRead, cancellationToken))
                {
                    var stream = await response.Content.ReadAsStreamAsync();
                    await response.CheckStatusCodes();

                    return stream.ReadAndDeserializeFromJson<TResponse>();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static async Task<TResponse> PostAsync<TRequest, TResponse>(this HttpClient httpClient,
           string uri, TRequest content, CancellationToken cancellationToken, string token = "")
        {
            try
            {
                using (var requestMessage = uri.CreateRequestHeaders(HttpMethod.Post, token))
                using ((await requestMessage.CreateRequestContent(content)).Content)
                using (var responseMessage = await httpClient.SendAsync(requestMessage,
                    HttpCompletionOption.ResponseHeadersRead, cancellationToken))
                {
                    await responseMessage.CheckStatusCodes();

                    return await responseMessage.DeserializeResponse<TResponse>();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static async Task<TResponse> PutAsync<TRequest, TResponse>(this HttpClient httpClient,
           string uri, TRequest content, CancellationToken cancellationToken, string token = "")
        {
            try
            {
                using (var requestMessage = uri.CreateRequestHeaders(HttpMethod.Put, token))
                using ((await requestMessage.CreateRequestContent(content)).Content)
                using (var responseMessage = await httpClient.SendAsync(requestMessage,
                    HttpCompletionOption.ResponseHeadersRead, cancellationToken))
                {
                    await responseMessage.CheckStatusCodes();

                    return await responseMessage.DeserializeResponse<TResponse>();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static async Task<string> DeleteAsync(this HttpClient httpClient,
          string uri, CancellationToken cancellationToken, string token = "")
        {
            try
            {
                using (var requestMessage = uri.CreateRequestHeaders(HttpMethod.Delete, token))
                using (var responseMessage = await httpClient.SendAsync(requestMessage,
                    HttpCompletionOption.ResponseHeadersRead, cancellationToken))
                {
                    await responseMessage.CheckStatusCodes();

                    return await responseMessage.Content.ReadAsStringAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
