using CSharpFunctionalExtensions;
using Newtonsoft.Json;
using PieceOfCake.Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace PieceOfCake.BlazorApp.Services
{
    public abstract class HttpRequestServiceBase
    {
        protected HttpClient HttpClient { get; private set; }

        public HttpRequestServiceBase(HttpClient httpClient)
        {
            HttpClient = httpClient;
        }

        public async Task<Result<T>> HandleGet<T>(string url)
        {
            var response = await HttpClient.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Envelope<T>>(content);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return Result.Success<T>(result.Result);
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                return Result.Failure<T>(result.ErrorMessage);
            }
            else
            {
                //handle 500 here
                var contentAsString = await response.Content.ReadAsStringAsync();
                throw new Exception(contentAsString);
            }
        }

        public async Task<Result<T>> HandlePost<T>(string url, T content)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            //request.Headers.Add("Accept-Language", "bg-BG");
            var contentAsJson = JsonConvert.SerializeObject(content);
            request.Content = new StringContent(contentAsJson);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await HttpClient.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<Envelope<T>>(responseContent);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return Result.Success<T>(result.Result);
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                return Result.Failure<T>(result.ErrorMessage);
            }
            else
            {
                //handle 500 here
                var contentAsString = await response.Content.ReadAsStringAsync();
                throw new Exception(contentAsString);
            }
        }

        public async Task<Result> HandleDelete(string url)
        {
            var response = await HttpClient.DeleteAsync(url);
            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Envelope>(responseContent);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return Result.Success();
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                return Result.Failure(result.ErrorMessage);
            }
            else
            {
                //handle 500 here
                var contentAsString = await response.Content.ReadAsStringAsync();
                throw new Exception(contentAsString);
            }
        }
    }
}
