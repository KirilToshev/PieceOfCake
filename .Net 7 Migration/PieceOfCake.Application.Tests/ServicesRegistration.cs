using Microsoft.Extensions.DependencyInjection;
using PieceOfCake.Core.Common.Resources;

namespace PieceOfCake.Application.Tests;
public static class ServicesRegistration
{
    public static IServiceProvider Register ()
    {
        IServiceCollection services = new ServiceCollection();
        services.AddLogging();
        services.AddLocalization();
        services.AddTransient<IResources, Resources>();
        return services.BuildServiceProvider();
    }
}
