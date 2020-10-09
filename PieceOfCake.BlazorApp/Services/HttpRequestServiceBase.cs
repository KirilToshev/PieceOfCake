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
    public abstract class HttpRequestServiceBase
    {
        protected HttpClient HttpClient { get; private set; }

        public HttpRequestServiceBase(HttpClient httpClient)
        {
            HttpClient = httpClient;
        }

        public async Task<Result<TResponseContent>> HandleGet<TResponseContent>(string url)
        {
            var response = await HttpClient.GetAsync(url);
            return await HandleGenericResponse<TResponseContent>(response);
        }

        public async Task<Result<TResponseContent>> HandlePost<TResponseContent>(string url, object content)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            //request.Headers.Add("Accept-Language", "bg-BG");
            var contentAsJson = JsonConvert.SerializeObject(content);
            request.Content = new StringContent(contentAsJson);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await HttpClient.SendAsync(request);
            return await HandleGenericResponse<TResponseContent>(response);
        }

        public async Task<Result<TResponseContent>> HandlePut<TResponseContent>(string url, object content)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, url);
            //request.Headers.Add("Accept-Language", "bg-BG");
            var contentAsJson = JsonConvert.SerializeObject(content);
            request.Content = new StringContent(contentAsJson);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await HttpClient.SendAsync(request);
            return await HandleGenericResponse<TResponseContent>(response);
        }

        public async Task<Result<TResponseContent>> HandlePatch<TResponseContent>(string url, object content = null)
        {
            var request = new HttpRequestMessage(HttpMethod.Patch, url);
            //request.Headers.Add("Accept-Language", "bg-BG");
            var contentAsJson = JsonConvert.SerializeObject(content);
            request.Content = new StringContent(contentAsJson);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await HttpClient.SendAsync(request);
            return await HandleGenericResponse<TResponseContent>(response);
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

        private async Task<Result<TResponseContent>> HandleGenericResponse<TResponseContent>(HttpResponseMessage response)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            var jsonMapping = JsonConvert.DeserializeObject<Envelope<TResponseContent>>(responseContent);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return Result.Success(jsonMapping.Result);
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                return Result.Failure<TResponseContent>(jsonMapping.ErrorMessage);
            }
            else
            {
                var contentAsString = await response.Content.ReadAsStringAsync();
                Console.WriteLine(contentAsString);
                return Result.Failure<TResponseContent>("A unhandled server exception occured. See the log or console for more information.");
            }
        }
    }
}
