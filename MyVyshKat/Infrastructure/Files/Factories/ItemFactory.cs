using System.Text.Json;
using MyVyshKat.Domain;

namespace MyVyshKat.Infrastructure.Files.Factories;

public abstract class ItemFactory : IItemFactory
{
    public abstract string Type { get; }
    
    public abstract InventoryItem Create(JsonElement element);

    protected static string ReadId(JsonElement element)
    {
        return element.GetRequiredProperty("id").GetString()!;
    }

    protected static string ReadName(JsonElement element)
    {
        return element.GetRequiredProperty("name").GetString()!;
    }

    protected static int ReadSimplicity(JsonElement element)
    {
        return element.GetRequiredProperty("simplicity").GetInt32();
    }

    protected static int ReadWear(JsonElement element)
    {
        return element.GetRequiredProperty("wearPercentage").GetInt32();
    }

    protected static double ReadDailyEnergyKwh(JsonElement element)
    {
        return element.GetRequiredProperty("dailyEnergyKwh").GetDouble();
    }
    
    
}