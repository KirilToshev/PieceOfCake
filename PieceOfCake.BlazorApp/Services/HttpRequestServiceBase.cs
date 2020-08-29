using CSharpFunctionalExtensions;
using Newtonsoft.Json;
using PieceOfCake.Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace PieceOfCake.BlazorApp.Services
{
    public abstract class HttpRequestServiceBase<T>
    {
        protected HttpClient HttpClient { get; private set; }

        public HttpRequestServiceBase(HttpClient httpClient)
        {
            HttpClient = httpClient;
        }

        public async Task<Result<U>> HandleGet<U>(string url)
        {
            var response = await HttpClient.GetAsync(url);
            return await HandleGenericResponse<U>(response);
        }

        public async Task<Result<T>> HandlePost(string url, T content)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            //request.Headers.Add("Accept-Language", "bg-BG");
            var contentAsJson = JsonConvert.SerializeObject(content);
            request.Content = new StringContent(contentAsJson);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await HttpClient.SendAsync(request);
            return await HandleGenericResponse<T>(response);
        }

        public async Task<Result<T>> HandlePut(string url, T content)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, url);
            //request.Headers.Add("Accept-Language", "bg-BG");
            var contentAsJson = JsonConvert.SerializeObject(content);
            request.Content = new StringContent(contentAsJson);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await HttpClient.SendAsync(request);
            return await HandleGenericResponse<T>(response);
        }

        public async Task<Result> HandleDelete(string url)
        {
            var response = await HttpClient.DeleteAsync(url);
            return await HandleResponse(response);
        }

        private async Task<Result> HandleResponse(HttpResponseMessage response)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            var jsonMapping = JsonConvert.DeserializeObject<Envelope>(responseContent);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return Result.Success();
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                return Result.Failure(jsonMapping.ErrorMessage);
            }
            else
            {
                var contentAsString = await response.Content.ReadAsStringAsync();
                Console.WriteLine(contentAsString);
                return Result.Failure("A unhandled server exception occured. See the log or console for more information.");
            }
        }

        private async Task<Result<W>> HandleGenericResponse<W>(HttpResponseMessage response)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            var jsonMapping = JsonConvert.DeserializeObject<Envelope<W>>(responseContent);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return Result.Success(jsonMapping.Result);
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                return Result.Failure<W>(jsonMapping.ErrorMessage);
            }
            else
            {
                var contentAsString = await response.Content.ReadAsStringAsync();
                Console.WriteLine(contentAsString);
                return Result.Failure<W>("A unhandled server exception occured. See the log or console for more information.");
            }
        }
    }
}
