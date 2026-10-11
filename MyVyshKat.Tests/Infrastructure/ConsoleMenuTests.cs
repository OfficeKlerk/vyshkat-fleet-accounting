using Microsoft.Extensions.DependencyInjection;
using MyVyshKat.Application;
using MyVyshKat.Infrastructure;
using MyVyshKat.Infrastructure.ConsoleUi;

namespace MyVyshKat.Tests.Infrastructure;

//меню тестируется на строках: вместо консоли через DI подставляются StringReader/StringWriter
public class ConsoleMenuTests : IDisposable
{
    private readonly string _jsonPath = Path.GetTempFileName();

    public ConsoleMenuTests()
    {
        File.WriteAllText(_jsonPath, """
        [
          { "type": "ElectricScooter", "id": "VK-1", "name": "Ninebot", "simplicity": 7, "wearPercentage": 10, "dailyEnergyKwh": 0.8 },
          { "type": "ElectricBike", "id": "VK-2", "name": "Shengmilo", "simplicity": 5, "wearPercentage": 90, "dailyEnergyKwh": 1.1 },
          { "type": "DockStation", "id": "VK-3", "name": "Dock", "dailyEnergyKwh": 3.5 }
        ]
        """);
    }

    public void Dispose()
    {
        File.Delete(_jsonPath);
    }

    private static (string Output, ServiceProvider Provider) Run(params string[] lines)
    {
        var output = new StringWriter();
        var services = new ServiceCollection().AddMyVyshKat();
        services.AddSingleton<TextReader>(new StringReader(string.Join('\n', lines) + '\n'));
        services.AddSingleton<TextWriter>(output);
        var provider = services.BuildServiceProvider();

        provider.GetRequiredService<ConsoleMenu>().Run();
        return (output.ToString(), provider);
    }

    [Fact]
    public void LoadFile_ThenReports_ShowResults()
    {
        var (output, provider) = Run("1", _jsonPath, "2", "3", "4", "0");
        using (provider)
        {
            Assert.Contains("[accepted] ElectricScooter VK-1 (Ninebot)", output);
            Assert.Contains("[rejected] ElectricBike VK-2 (Shengmilo): Wear 90%", output);
            Assert.Contains("Accepted 2 of 3", output);
            Assert.Contains("Units on balance: 2", output);
            Assert.Contains("Total daily energy consumption: 4.3 kWh", output);
            Assert.Contains("VK-1  Ninebot  simplicity 7/10", output);
            Assert.Contains("VK-3  Dock", output);
            Assert.Equal(2, provider.GetRequiredService<Fleet>().GetItems().Count);
        }
    }

    [Fact]
    public void EmptyFleet_ReportsSayNothing()
    {
        var (output, provider) = Run("2", "3", "4", "0");
        using (provider)
        {
            Assert.Contains("Units on balance: 0", output);
            Assert.Contains("none", output);
            Assert.Contains("nothing on balance", output);
        }
    }

    [Fact]
    public void MissingFileAndEmptyPath_DoNotCrash()
    {
        var (output, provider) = Run("1", "/no/such/file.json", "1", "", "0");
        using (provider)
        {
            Assert.Contains("Cannot load file", output);
            Assert.Contains("Path is empty", output);
        }
    }

    [Fact]
    public void UnknownOption_ShowsHint()
    {
        var (output, provider) = Run("9", "0");
        using (provider)
        {
            Assert.Contains("Unknown option", output);
        }
    }

    [Fact]
    public void EndOfInput_ExitsGracefully()
    {
        var (output, provider) = Run("2");
        using (provider)
        {
            Assert.EndsWith("Bye!" + Environment.NewLine, output);
        }
    }
}
