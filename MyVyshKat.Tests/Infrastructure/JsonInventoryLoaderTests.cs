using System.Text.Json;
using MyVyshKat.Application;
using MyVyshKat.Domain.Things;
using MyVyshKat.Domain.Vehicles;
using MyVyshKat.Infrastructure.Files;
using MyVyshKat.Infrastructure.Files.Factories;
using MyVyshKat.Tests.Fakes;

namespace MyVyshKat.Tests.Infrastructure;

public class JsonInventoryLoaderTests
{
    private static JsonInventoryLoader CreateLoader(Fleet fleet)
    {
        var factories = new IItemFactory[]
        {
            new ElectricScooterFactory(), new ElectricBikeFactory(), new BicycleFactory(),
            new HelmetFactory(), new DockStationFactory(), new ChargingCabinetFactory(),
        };
        return new JsonInventoryLoader(fleet, factories);
    }

    [Fact]
    public void EveryFactory_CreatesItsType()
    {
        var fleet = new Fleet(FakeServiceCenter.AcceptAll());
        var json = """
        [
          { "type": "ElectricScooter", "id": "1", "name": "a", "simplicity": 7, "wearPercentage": 1, "dailyEnergyKwh": 0.5 },
          { "type": "ElectricBike", "id": "2", "name": "b", "simplicity": 5, "wearPercentage": 1, "dailyEnergyKwh": 1.0 },
          { "type": "Bicycle", "id": "3", "name": "c", "simplicity": 9, "wearPercentage": 1 },
          { "type": "Helmet", "id": "4", "name": "d" },
          { "type": "DockStation", "id": "5", "name": "e", "dailyEnergyKwh": 3.5 },
          { "type": "ChargingCabinet", "id": "6", "name": "f", "dailyEnergyKwh": 2.0 }
        ]
        """;

        var results = CreateLoader(fleet).Load(json);

        Assert.All(results, result => Assert.True(result.Accepted));
        var items = fleet.GetItems();
        Assert.IsType<ElectricScooter>(items[0]);
        Assert.IsType<ElectricBike>(items[1]);
        Assert.IsType<Bicycle>(items[2]);
        Assert.IsType<Helmet>(items[3]);
        Assert.IsType<DockStation>(items[4]);
        Assert.IsType<ChargingCabinet>(items[5]);
    }

    [Fact]
    public void ItemsGoThroughInspection()
    {
        //загрузчик не может положить предмет в парк в обход техосмотра
        var serviceCenter = FakeServiceCenter.RejectAll("Closed");
        var fleet = new Fleet(serviceCenter);

        var results = CreateLoader(fleet).Load("""[ { "type": "Helmet", "id": "1", "name": "h" } ]""");

        Assert.False(Assert.Single(results).Accepted);
        Assert.Equal("Closed", results[0].Reason);
        Assert.Single(serviceCenter.Inspected);
        Assert.Empty(fleet.GetItems());
    }

    [Theory]
    [InlineData("""{ "type": "Monowheel", "id": "1", "name": "m" }""", "Unknown type 'Monowheel'")]
    [InlineData("""{ "id": "1", "name": "no type" }""", "Missing field 'type'")]
    [InlineData("""{ "type": "Bicycle", "id": "1", "name": "b", "wearPercentage": 0 }""", "Missing field 'simplicity'")]
    [InlineData("""{ "type": "Bicycle", "id": "1", "name": "b", "simplicity": 15, "wearPercentage": 0 }""", "Simplicity must be between 1 and 10")]
    [InlineData("""{ "type": "Bicycle", "id": "1", "name": "b", "simplicity": "seven", "wearPercentage": 0 }""", "Number")]
    [InlineData("42", "Object")]
    public void BadItem_IsSkippedWithReason(string itemJson, string expectedReasonPart)
    {
        var fleet = new Fleet(FakeServiceCenter.AcceptAll());

        var results = CreateLoader(fleet).Load($"[ {itemJson}, {{ \"type\": \"Helmet\", \"id\": \"ok\", \"name\": \"h\" }} ]");

        Assert.False(results[0].Accepted);
        Assert.Contains(expectedReasonPart, results[0].Reason);
        //остальные предметы всё равно загружаются
        Assert.True(results[1].Accepted);
        Assert.Single(fleet.GetItems());
    }

    [Fact]
    public void BrokenJson_Throws()
    {
        var loader = CreateLoader(new Fleet(FakeServiceCenter.AcceptAll()));

        Assert.ThrowsAny<JsonException>(() => loader.Load("[ broken"));
        Assert.Throws<InvalidDataException>(() => loader.Load("""{ "type": "Helmet" }"""));
    }

    [Fact]
    public void LoadFromFile_ReadsFile()
    {
        var path = Path.GetTempFileName();
        try
        {
            File.WriteAllText(path, """[ { "type": "Helmet", "id": "1", "name": "h" } ]""");
            var fleet = new Fleet(FakeServiceCenter.AcceptAll());

            var results = CreateLoader(fleet).LoadFromFile(path);

            Assert.True(Assert.Single(results).Accepted);
        }
        finally
        {
            File.Delete(path);
        }
    }
}
