using System.Text.Json;
using MyVyshKat.Domain;
using MyVyshKat.Domain.Vehicles;

namespace MyVyshKat.Infrastructure.Files.Factories;

public class ElectricScooterFactory : ItemFactory
{
    public override string Type => "ElectricScooter";

    public override InventoryItem Create(JsonElement element)
    {
        return new ElectricScooter(ReadId(element),  ReadName(element), 
                                   ReadSimplicity(element), ReadWear(element), 
                                   ReadDailyEnergyKwh(element));
    }
}
