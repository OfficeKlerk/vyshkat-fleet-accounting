using MyVyshKat.Domain;
using MyVyshKat.Domain.Vehicles;

namespace MyVyshKat.Application;

public class ServiceCenter : IServiceCenter
{
    private readonly double _vehicleEnergyLimit;
    private readonly double _thingEnergyLimit;
    private readonly int _wearPercentageLimit;

    public ServiceCenter(double vehicleEnergyLimit, double thingEnergyLimit, int wearPercentageLimit)
    {
        _vehicleEnergyLimit = vehicleEnergyLimit;
        _thingEnergyLimit = thingEnergyLimit;
        _wearPercentageLimit = wearPercentageLimit;
    }

    public InspectionResult ApplyItem(InventoryItem item)
    {
        if (item is IEnergyConsumer consumer)
        {
            var energyLimit = item is Vehicle ? _vehicleEnergyLimit : _thingEnergyLimit;
            if (consumer.DailyEnergyKwh > energyLimit)
            {
                return new InspectionResult(false,
                    $"Daily energy consumption {consumer.DailyEnergyKwh} kWh exceeds the limit of {energyLimit} kWh");
            }
        }

        if (item is Vehicle vehicle && vehicle.WearPercentage > _wearPercentageLimit)
        {
            return new InspectionResult(false,
                $"Wear {vehicle.WearPercentage}% exceeds the allowed {_wearPercentageLimit}%");
        }

        return new InspectionResult(true);
    }
}
