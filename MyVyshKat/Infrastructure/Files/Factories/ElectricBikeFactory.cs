using System.Text.Json;
using MyVyshKat.Domain;
using MyVyshKat.Domain.Vehicles;

namespace MyVyshKat.Infrastructure.Files.Factories;

public class ElectricBikeFactory : ItemFactory
{
    public override string Type => "ElectricBike";

    public override InventoryItem Create(JsonElement element)
    {
        return new ElectricBike(ReadId(element), ReadName(element),
            ReadSimplicity(element), ReadWear(element), ReadDailyEnergyKwh(element));
    }
}
