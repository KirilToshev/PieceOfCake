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

namespace PieceOfCake.BlazorApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Services.AddScoped(sp => new HttpClient 
            { 
                BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) 
            });

            builder.Services.AddHttpClient<IMeasureUnitHttpService, MeasureUnitHttpService>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:44312/");
            });

            builder.Services.AddHttpClient<IProductHttpService, ProductHttpService>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:44312/");
            });

            builder.Services.AddHttpClient<IDishHttpService, DishHttpService>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:44312/");
            });

            builder.Services.AddHttpClient<IMenuHttpService, MenuHttpService>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:44312/");
            });

            builder.Services.AddSingleton<IEventsService, EventsService>();

            await builder.Build().RunAsync();
        }
    }
}
