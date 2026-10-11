using MyVyshKat.Domain;
using MyVyshKat.Domain.Things;
using MyVyshKat.Domain.Vehicles;
using MyVyshKat.Tests.Fakes;

namespace MyVyshKat.Tests.Domain;

public class ModelValidationTests
{
    [Fact]
    public void ValidVehicle_KeepsAllValues()
    {
        var scooter = new ElectricScooter("VK-1", "Ninebot", 7, 10, 0.8);

        Assert.Equal("VK-1", scooter.Id);
        Assert.Equal("Ninebot", scooter.Name);
        Assert.Equal(7, scooter.Simplicity);
        Assert.Equal(10, scooter.WearPercentage);
        Assert.Equal(0.8, scooter.DailyEnergyKwh);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(11)]
    public void Vehicle_SimplicityOutOfRange_Throws(int simplicity)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Samples.Bicycle(simplicity: simplicity));
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(101)]
    public void Vehicle_WearOutOfRange_Throws(int wear)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Samples.Bicycle(wear: wear));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(100)]
    public void Vehicle_WearBounds_AreAllowed(int wear)
    {
        Assert.Equal(wear, Samples.Bicycle(wear: wear).WearPercentage);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void InventoryItem_BlankIdOrName_Throws(string blank)
    {
        Assert.Throws<ArgumentException>(() => new Helmet(blank, "Helmet"));
        Assert.Throws<ArgumentException>(() => new Helmet("H-1", blank));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(double.NaN)]
    [InlineData(double.PositiveInfinity)]
    public void EnergyConsumers_RequirePositiveEnergy(double energy)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Samples.Scooter(energy: energy));
        Assert.Throws<ArgumentOutOfRangeException>(() => Samples.EBike(energy: energy));
        Assert.Throws<ArgumentOutOfRangeException>(() => Samples.Dock(energy: energy));
        Assert.Throws<ArgumentOutOfRangeException>(() => Samples.Cabinet(energy: energy));
    }

    [Fact]
    public void OnlyRealConsumers_ImplementIEnergyConsumer()
    {
        Assert.True(typeof(IEnergyConsumer).IsAssignableFrom(typeof(ElectricScooter)));
        Assert.True(typeof(IEnergyConsumer).IsAssignableFrom(typeof(ElectricBike)));
        Assert.True(typeof(IEnergyConsumer).IsAssignableFrom(typeof(DockStation)));
        Assert.True(typeof(IEnergyConsumer).IsAssignableFrom(typeof(ChargingCabinet)));
        Assert.False(typeof(IEnergyConsumer).IsAssignableFrom(typeof(Bicycle)));
        Assert.False(typeof(IEnergyConsumer).IsAssignableFrom(typeof(Helmet)));
    }
}
