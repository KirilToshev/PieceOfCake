using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Newtonsoft.Json;
using PieceOfCake.Shared.ViewModels;
using System;
using System.Globalization;
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
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await SendRequest(request);
            return await HandleGenericResponse<TResponseContent>(response);
        }

        public async Task<Result<TResponseContent>> HandlePost<TResponseContent>(string url, object content)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            var response = await SendRequest(request, content);
            return await HandleGenericResponse<TResponseContent>(response);
        }

        public async Task<Result<TResponseContent>> HandlePut<TResponseContent>(string url, object content)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, url);
            var response = await SendRequest(request, content);
            return await HandleGenericResponse<TResponseContent>(response);
        }

        public async Task<Result<TResponseContent>> HandlePatch<TResponseContent>(string url, object content = null)
        {
            var request = new HttpRequestMessage(HttpMethod.Patch, url);
            var response = await SendRequest(request, content);
            return await HandleGenericResponse<TResponseContent>(response);
        }

        public async Task<Result> HandleDelete(string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Delete, url);
            var response = await SendRequest(request);
            return await HandleResponse(response);
        }

        private async Task<Result<HttpResponseMessage>> SendRequest(HttpRequestMessage request, object content = null)
        {
            request.Headers.Add("Accept-Language", CultureInfo.CurrentCulture.Name);

            if(content != null)
            {
                var contentAsJson = JsonConvert.SerializeObject(content);
                request.Content = new StringContent(contentAsJson);
                request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            }

            try
            {
                return Result.Success(await HttpClient.SendAsync(request));
            }
            catch (AccessTokenNotAvailableException exception)
            {
                exception.Redirect();
                //TODO: Translation
                return Result.Failure<HttpResponseMessage>("Unable to acquire access token. Please login.");
            }
        }

        private async Task<Result> HandleResponse(Result<HttpResponseMessage> responseResult)
        {
            if (responseResult.IsFailure)
                return responseResult;

            var response = responseResult.Value;

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
                return Result.Failure("An unhandled server exception occured. See the log or console for more information.");
            }
        }

        private async Task<Result<TResponseContent>> HandleGenericResponse<TResponseContent>(Result<HttpResponseMessage> responseResult)
        {
            if (responseResult.IsFailure)
                return responseResult.ConvertFailure<TResponseContent>();

            var response = responseResult.Value;

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
                return Result.Failure<TResponseContent>("An unhandled server exception occured. See the log or console for more information.");
            }
        }
    }
}
