using MyVyshKat.Application;
using MyVyshKat.Tests.Fakes;

namespace MyVyshKat.Tests.Application;

public class ServiceCenterTests
{
    private readonly ServiceCenter _serviceCenter = new ServiceCenter(
        vehicleEnergyLimit: 1.5, thingEnergyLimit: 5.0, wearPercentageLimit: 70);

    [Fact]
    public void HealthyScooter_IsAccepted()
    {
        var result = _serviceCenter.ApplyItem(Samples.Scooter(wear: 10, energy: 0.8));

        Assert.True(result.Accepted);
        Assert.Null(result.RejectionReason);
    }

    [Fact]
    public void WearAtLimit_IsAccepted()
    {
        Assert.True(_serviceCenter.ApplyItem(Samples.Bicycle(wear: 70)).Accepted);
    }

    [Fact]
    public void WearAboveLimit_IsRejectedWithReason()
    {
        var result = _serviceCenter.ApplyItem(Samples.Bicycle(wear: 71));

        Assert.False(result.Accepted);
        Assert.Contains("Wear", result.RejectionReason);
    }

    [Fact]
    public void VehicleAboveVehicleEnergyLimit_IsRejected()
    {
        var result = _serviceCenter.ApplyItem(Samples.Scooter(energy: 2.4));

        Assert.False(result.Accepted);
        Assert.Contains("energy", result.RejectionReason);
    }

    [Fact]
    public void ThingUsesItsOwnEnergyLimit()
    {
        //3.5 кВт·ч — много для самоката, но нормально для док-станции
        Assert.False(_serviceCenter.ApplyItem(Samples.Scooter(energy: 3.5)).Accepted);
        Assert.True(_serviceCenter.ApplyItem(Samples.Dock(energy: 3.5)).Accepted);
        Assert.False(_serviceCenter.ApplyItem(Samples.Cabinet(energy: 5.1)).Accepted);
    }

    [Fact]
    public void ItemWithoutWearAndEnergy_IsAccepted()
    {
        Assert.True(_serviceCenter.ApplyItem(Samples.Helmet()).Accepted);
    }
}
