using MyVyshKat.Application;
using MyVyshKat.Tests.Fakes;

namespace MyVyshKat.Tests.Application;

public class FleetReportsTests
{
    private readonly Fleet _fleet = new Fleet(FakeServiceCenter.AcceptAll());
    private readonly FleetReports _reports;

    public FleetReportsTests()
    {
        _reports = new FleetReports(_fleet, new SimplicityRule(6));
    }

    [Fact]
    public void EmptyFleet_HasZeroUnitsAndEnergy()
    {
        Assert.Equal(0, _reports.GetItemsQuantity());
        Assert.Equal(0, _reports.GetCommonEnergyConsumption());
        Assert.Empty(_reports.GetReport());
        Assert.Empty(_reports.GetBeginnersItems());
    }

    [Fact]
    public void Energy_IsSummedOverAllConsumers()
    {
        _fleet.AddItem(Samples.Scooter(id: "1", energy: 0.8));
        _fleet.AddItem(Samples.EBike(id: "2", energy: 1.2));
        _fleet.AddItem(Samples.Bicycle(id: "3"));
        _fleet.AddItem(Samples.Helmet(id: "4"));
        _fleet.AddItem(Samples.Dock(id: "5", energy: 3.5));

        Assert.Equal(5, _reports.GetItemsQuantity());
        Assert.Equal(5.5, _reports.GetCommonEnergyConsumption(), precision: 6);
    }

    [Fact]
    public void BeginnersItems_ContainOnlySuitableVehicles()
    {
        _fleet.AddItem(Samples.Scooter(id: "easy", simplicity: 6));
        _fleet.AddItem(Samples.EBike(id: "hard", simplicity: 5));
        _fleet.AddItem(Samples.Bicycle(id: "very-easy", simplicity: 9));
        _fleet.AddItem(Samples.Helmet(id: "helmet"));

        var ids = _reports.GetBeginnersItems().Select(item => item.Id).ToList();

        Assert.Equal(new[] { "easy", "very-easy" }, ids);
        Assert.Equal("Simplicity >= 6", _reports.BeginnerRuleDescription);
    }

    [Fact]
    public void Report_ListsIdAndNameOfEverything()
    {
        _fleet.AddItem(Samples.Scooter(id: "S-1"));
        _fleet.AddItem(Samples.Helmet(id: "H-1"));

        var report = _reports.GetReport();

        Assert.Equal(new[] { ("S-1", "Scooter S-1"), ("H-1", "Helmet H-1") },
            report.Select(entry => (entry.Id, entry.Name)));
    }
}
