using System.Text.Json;
using MyVyshKat.Domain;
using MyVyshKat.Domain.Vehicles;

namespace MyVyshKat.Infrastructure.Files.Factories;

public class BicycleFactory : ItemFactory
{
    public override string Type => "Bicycle";

    public override InventoryItem Create(JsonElement element)
    {
        return new Bicycle(ReadId(element), ReadName(element),
            ReadSimplicity(element), ReadWear(element));
    }
}
