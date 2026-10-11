using Microsoft.Extensions.DependencyInjection;
using MyVyshKat.Application;
using MyVyshKat.Infrastructure.ConsoleUi;
using MyVyshKat.Infrastructure.Files;
using MyVyshKat.Infrastructure.Files.Factories;

namespace MyVyshKat.Infrastructure;

//здесь собираются все зависимости приложения
public static class DependencyInjection
{
    public static IServiceCollection AddMyVyshKat(this IServiceCollection services)
    {
        //правила приёмки и выдачи новичкам
        services.AddSingleton<IServiceCenter>(_ => new ServiceCenter(
            vehicleEnergyLimit: 1.5,
            thingEnergyLimit: 5.0,
            wearPercentageLimit: 70));
        services.AddSingleton<IBeginnerRule>(_ => new SimplicityRule(minSimplicity: 6));

        //парк
        services.AddSingleton<Fleet>();
        services.AddSingleton<FleetReports>();

        //фабрики для загрузки из json;
        services.AddSingleton<IItemFactory, ElectricScooterFactory>();
        services.AddSingleton<IItemFactory, ElectricBikeFactory>();
        services.AddSingleton<IItemFactory, BicycleFactory>();
        services.AddSingleton<IItemFactory, HelmetFactory>();
        services.AddSingleton<IItemFactory, DockStationFactory>();
        services.AddSingleton<IItemFactory, ChargingCabinetFactory>();
        services.AddSingleton<JsonInventoryLoader>();

        //консоль; потоки ввода/вывода тоже зависимости — в тестах их можно заменить строками
        services.AddSingleton(Console.In);
        services.AddSingleton(Console.Out);
        services.AddSingleton<ConsoleMenu>();

        return services;
    }
}
