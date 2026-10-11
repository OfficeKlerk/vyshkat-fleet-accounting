using System.Text.Json;

namespace MyVyshKat.Infrastructure.Files;

internal static class JsonElementExtensions
{
    //как GetProperty, но в ошибке видно, какого именно поля не хватает
    public static JsonElement GetRequiredProperty(this JsonElement element, string name)
    {
        if (!element.TryGetProperty(name, out var value))
        {
            throw new KeyNotFoundException($"Missing field '{name}'");
        }

        return value;
    }
}
