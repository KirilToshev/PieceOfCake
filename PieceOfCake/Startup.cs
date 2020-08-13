using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using PieceOfCake.Core.Common;
using PieceOfCake.Core.DomainServices;
using PieceOfCake.Core.DomainServices.Interfaces;
using PieceOfCake.Core.Persistence;
using PieceOfCake.Core.Resources;
using PieceOfCake.Persistence;

namespace PieceOfCake.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<PocDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("PocDbContext")));
                //options.UseSqlite("DataSource=:memory:"));

            services.AddControllers(setup => {
                setup.ReturnHttpNotAcceptable = true;
            })
            .AddNewtonsoftJson()
            .AddXmlDataContractSerializerFormatters();

            services.AddLocalization();
            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture(Common.SupportedLanguages[0]);
                options.AddSupportedCultures(Common.SupportedLanguages);
                options.AddSupportedUICultures(Common.SupportedLanguages);
            });

            services.AddSingleton<IResources, Resources>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IMeasureUnitDomainService, MeasureUnitDomainService>();
            services.AddTransient<IProductDomainService, ProductDomainService>();
            services.AddTransient<IDishDomainService, DishDomainService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            var options = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(options.Value);

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
