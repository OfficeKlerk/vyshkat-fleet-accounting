using System.Collections.ObjectModel;
using MyVyshKat.Domain;
using MyVyshKat.Domain.Vehicles;

namespace MyVyshKat.Application;

public class FleetReports
{
    private readonly Fleet _fleet;
    private readonly IBeginnerRule _beginnerRule;

    public FleetReports(Fleet fleet, IBeginnerRule beginnerRule)
    {
        _fleet = fleet;
        _beginnerRule = beginnerRule;
    }

    public string BeginnerRuleDescription => _beginnerRule.Description;
    
    public ReadOnlyCollection<(string Id, string Name)> GetReport()
    {
        return _fleet.GetItems().Select(x => (x.Id, x.Name)).ToList().AsReadOnly();
    }
    
    public ReadOnlyCollection<InventoryItem> GetBeginnersItems()
    {
        return _fleet.GetItems().Where(x => x is Vehicle vehicle && 
                                            _beginnerRule.IsSuitable(vehicle)).ToList().AsReadOnly();
    }

    public int GetItemsQuantity()
    {
        return _fleet.GetItems().Count;
    }

    public double GetCommonEnergyConsumption()
    {
        return _fleet.GetItems().Sum(x => x is IEnergyConsumer consumer ? consumer.DailyEnergyKwh : 0);
    }
}