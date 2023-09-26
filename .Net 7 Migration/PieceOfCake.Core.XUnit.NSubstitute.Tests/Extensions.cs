using Microsoft.Extensions.DependencyInjection;
using PieceOfCake.Core.Resources;

namespace PieceOfCake.UnitTests;

internal static class Extensions
{
    public static void AddResources(this IServiceCollection services)
    {
        services.AddLogging();
        services.AddLocalization();

        services.AddTransient<IResources, Resources>();
    }
}
