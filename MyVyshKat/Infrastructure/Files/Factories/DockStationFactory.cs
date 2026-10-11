using System.Text.Json;
using MyVyshKat.Domain;
using MyVyshKat.Domain.Things;

namespace MyVyshKat.Infrastructure.Files.Factories;

public class DockStationFactory : ItemFactory
{
    public override string Type => "DockStation";

    public override InventoryItem Create(JsonElement element)
    {
        return new DockStation(ReadId(element), ReadName(element), ReadDailyEnergyKwh(element));
    }
}
