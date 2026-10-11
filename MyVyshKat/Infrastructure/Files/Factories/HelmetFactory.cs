using System.Text.Json;
using MyVyshKat.Domain;
using MyVyshKat.Domain.Things;

namespace MyVyshKat.Infrastructure.Files.Factories;

public class HelmetFactory : ItemFactory
{
    public override string Type => "Helmet";

    public override InventoryItem Create(JsonElement element)
    {
        return new Helmet(ReadId(element), ReadName(element));
    }
}
