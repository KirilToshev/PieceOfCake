using AutoFixture;
using Microsoft.Extensions.DependencyInjection;
using PieceOfCake.Core.Common.Resources;

namespace PieceOfCake.Tests.Common;
public class TestsCommon
{
    private readonly IResources _resources;
    private readonly Fixture _fixture;
    private IServiceProvider _serviceProvider;

    public TestsCommon (Func<IServiceProvider> generateServices)
    {
        _fixture = new Fixture();
        _serviceProvider = generateServices.Invoke();
        _resources = _serviceProvider.GetService<IResources>()!;
    }

    public Fixture Fixture => _fixture;
    public IResources Resources => _resources;

    public T GetRequiredService<T> () where T : notnull => _serviceProvider.GetRequiredService<T>();
}
