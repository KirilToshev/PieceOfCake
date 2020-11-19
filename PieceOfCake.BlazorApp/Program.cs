using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PieceOfCake.BlazorApp.Services;
using PieceOfCake.BlazorApp.Services.Interfaces;
using Microsoft.JSInterop;
using System.Globalization;
using PieceOfCake.BlazorApp.Resources;
using PieceOfCake.BlazorApp.Services.MessageHandlers;

namespace PieceOfCake.BlazorApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Services.AddTransient<PieceOfCakeApiMessageHandler>();

            builder.Services.AddOidcAuthentication(options =>
            {
                builder.Configuration.Bind("OidcConfiguration", options.ProviderOptions);
            });

            builder.Services.AddScoped(sp => new HttpClient 
            { 
                BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) 
            });

            builder.Services.AddHttpClient<IMeasureUnitHttpService, MeasureUnitHttpService>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:44312/");
            }).AddHttpMessageHandler<PieceOfCakeApiMessageHandler>();

            builder.Services.AddHttpClient<IProductHttpService, ProductHttpService>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:44312/");
            }).AddHttpMessageHandler<PieceOfCakeApiMessageHandler>();

            builder.Services.AddHttpClient<IDishHttpService, DishHttpService>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:44312/");
            }).AddHttpMessageHandler<PieceOfCakeApiMessageHandler>();

            builder.Services.AddHttpClient<IMenuHttpService, MenuHttpService>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:44312/");
            }).AddHttpMessageHandler<PieceOfCakeApiMessageHandler>();

            builder.Services.AddSingleton<IEventsService, EventsService>();
            builder.Services.AddSingleton<IResources, Resources.Resources>();
            builder.Services.AddLocalization();

            var host = builder.Build();
            var jsInterop = host.Services.GetRequiredService<IJSRuntime>();
            var result = await jsInterop.InvokeAsync<string>("blazorCulture.get");
            if (result != null)
            {
                var culture = new CultureInfo(result);
                CultureInfo.DefaultThreadCurrentCulture = culture;
                CultureInfo.DefaultThreadCurrentUICulture = culture;
            }

            await host.RunAsync();
        }
    }
}
