using Microsoft.Extensions.DependencyInjection;
using MyVyshKat.Application;
using MyVyshKat.Infrastructure;
using MyVyshKat.Infrastructure.ConsoleUi;
using MyVyshKat.Infrastructure.Files;
using MyVyshKat.Infrastructure.Files.Factories;
using MyVyshKat.Tests.Fakes;

namespace MyVyshKat.Tests.Infrastructure;

public class DependencyInjectionTests
{
    [Fact]
    public void Container_ResolvesWholeApplication()
    {
        using var provider = new ServiceCollection().AddMyVyshKat()
            .BuildServiceProvider(new ServiceProviderOptions { ValidateOnBuild = true });

        Assert.NotNull(provider.GetRequiredService<ConsoleMenu>());
        Assert.IsType<ServiceCenter>(provider.GetRequiredService<IServiceCenter>());
        Assert.Equal(6, provider.GetServices<IItemFactory>().Count());
        //парк один на всё приложение
        Assert.Same(provider.GetRequiredService<Fleet>(), provider.GetRequiredService<Fleet>());
    }

    [Fact]
    public void ServiceCenter_CanBeReplacedThroughContainer()
    {
        //регистрация после AddMyVyshKat перекрывает настоящий техосмотр
        var fake = FakeServiceCenter.RejectAll("Replaced through DI");
        var services = new ServiceCollection().AddMyVyshKat();
        services.AddSingleton<IServiceCenter>(fake);
        using var provider = services.BuildServiceProvider();

        var fleet = provider.GetRequiredService<Fleet>();
        var result = fleet.AddItem(Samples.Scooter());

        Assert.False(result.Accepted);
        Assert.Equal("Replaced through DI", result.RejectionReason);
        Assert.Single(fake.Inspected);
        Assert.Empty(fleet.GetItems());
    }

    [Fact]
    public void LoaderFromContainer_UsesReplacedServiceCenter()
    {
        var fake = FakeServiceCenter.AcceptAll();
        var services = new ServiceCollection().AddMyVyshKat();
        services.AddSingleton<IServiceCenter>(fake);
        using var provider = services.BuildServiceProvider();

        //износ 99% настоящий техосмотр отклонил бы, а заглушка принимает
        provider.GetRequiredService<JsonInventoryLoader>().Load(
            """[ { "type": "Bicycle", "id": "1", "name": "b", "simplicity": 5, "wearPercentage": 99 } ]""");

        Assert.Single(provider.GetRequiredService<Fleet>().GetItems());
        Assert.Single(fake.Inspected);
    }
}
