using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PieceOfCake.Core.Resources;
using PieceOfCake.Core.Common;
using PieceOfCake.Core.Resources;

namespace PieceOfCake.UnitTests
{
    internal static class Setup
    {
        public static void AddResources(this IServiceCollection services)
        {
            services.AddLogging(logging => logging.AddConsole());
            services.AddLocalization();

            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture(Common.SupportedLanguages[0]);
                options.AddSupportedCultures(Common.SupportedLanguages);
                options.AddSupportedUICultures(Common.SupportedLanguages);
            });

            services.AddTransient<IResources, Resources>();
        }
    }
}
