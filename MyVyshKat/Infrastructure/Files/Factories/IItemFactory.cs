using System.Text.Json;
using MyVyshKat.Domain;

namespace MyVyshKat.Infrastructure.Files.Factories;

public interface IItemFactory
{
    string Type { get; }
    InventoryItem Create(JsonElement element);
}
