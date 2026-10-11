using System.Text.Json;
using MyVyshKat.Domain;
using MyVyshKat.Domain.Things;

namespace MyVyshKat.Infrastructure.Files.Factories;

public class ChargingCabinetFactory : ItemFactory
{
    public override string Type => "ChargingCabinet";

    public override InventoryItem Create(JsonElement element)
    {
        return new ChargingCabinet(ReadId(element), ReadName(element), ReadDailyEnergyKwh(element));
    }
}
