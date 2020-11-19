using AutoMapper;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using PieceOfCake.Api.Mapping;
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
            //Authentication and Authorization
            var requireAuthenticatedUserPolicy = new AuthorizationPolicyBuilder()
                 .RequireAuthenticatedUser()
                 .Build();

            services.AddAuthentication(
                IdentityServerAuthenticationDefaults.AuthenticationScheme)
            .AddIdentityServerAuthentication(options =>
            {
                options.Authority = "https://localhost:44333/";
                options.ApiName = "pieceOfCakeApi";
            });

            //SQL Server
            services.AddDbContext<PocDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("PocDbContext"))
                .UseLazyLoadingProxies());
            //options.UseSqlite("DataSource=:memory:"));

            //Json Conversion
            services.AddControllers(setup =>
            {
                setup.ReturnHttpNotAcceptable = true;
                setup.Filters.Add(new AuthorizeFilter(requireAuthenticatedUserPolicy));
            })
            .AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore)
            .AddXmlDataContractSerializerFormatters();

            //CORS
            services.AddCors(options => options.AddPolicy("AllowEverything",
                    builder => builder.AllowAnyOrigin()
                                      .AllowAnyMethod()
                                      .AllowAnyHeader()));

            //Localization
            services.AddLocalization();
            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture(Common.SupportedLanguages[0]);
                options.AddSupportedCultures(Common.SupportedLanguages);
                options.AddSupportedUICultures(Common.SupportedLanguages);
            });

            services.AddSingleton<IResources, Resources>();
            var sp = services.BuildServiceProvider();

            //AutoMapper Configuration
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AutoMapperMappingProfile(sp.GetService<IResources>()));
            });
            IMapper mapper = mapperConfig.CreateMapper();

            //Register services
            services.AddSingleton(mapper);
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IMeasureUnitDomainService, MeasureUnitDomainService>();
            services.AddTransient<IProductDomainService, ProductDomainService>();
            services.AddTransient<IDishDomainService, DishDomainService>();
            services.AddTransient<IMenuDomainService, MenuDomainService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseExceptionHandler("/error-local-development");
            }
            else
            {
                app.UseExceptionHandler("/error");
            }

            app.UseHttpsRedirection();

            var options = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(options.Value);

            app.UseRouting();

            app.UseCors("AllowEverything");

            app.UseAuthorization();

            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
