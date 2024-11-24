using Microsoft.Extensions.DependencyInjection;
using PieceOfCake.Core.Common.Resources;

namespace PieceOfCake.Application.Tests;

internal static class Extensions
{
    public static void AddResources (this IServiceCollection services)
    {
        services.AddLogging();
        services.AddLocalization();

        services.AddTransient<IResources, Resources>();
    }
}
