using System.Text.Json;
using MyVyshKat.Application;
using MyVyshKat.Infrastructure.Files.Factories;

namespace MyVyshKat.Infrastructure.Files;

//класс парсит данные из json-файла
public class JsonInventoryLoader
{
    private readonly Fleet _fleet;
    private readonly Dictionary<string, IItemFactory> _factories;

    public JsonInventoryLoader(Fleet fleet, IEnumerable<IItemFactory> factories)
    {
        _fleet = fleet;
        _factories = factories.ToDictionary(factory => factory.Type);
    }

    public List<LoadResult> LoadFromFile(string path)
    {
        return Load(File.ReadAllText(path));
    }

    //если сам json битый или это не массив - бросаем исключение
    //если плох отдельный предмет - пропускаем его и пишем причину в результат
    public List<LoadResult> Load(string json)
    {
        using var document = JsonDocument.Parse(json);
        if (document.RootElement.ValueKind != JsonValueKind.Array)
        {
            throw new InvalidDataException("Root element must be an array of items");
        }

        var results = new List<LoadResult>();
        var position = 0;
        foreach (var element in document.RootElement.EnumerateArray())
        {
            position++;
            results.Add(LoadItem(element, position));
        }

        return results;
    }

    private LoadResult LoadItem(JsonElement element, int position)
    {
        var label = $"Item #{position}";
        try
        {
            var type = element.GetRequiredProperty("type").GetString();
            if (type is null || !_factories.TryGetValue(type, out var factory))
            {
                return new LoadResult(label, false, $"Unknown type '{type}'");
            }

            var item = factory.Create(element);
            label = $"{type} {item.Id} ({item.Name})";

            var inspection = _fleet.AddItem(item);
            return new LoadResult(label, inspection.Accepted, inspection.RejectionReason);
        }
        catch (Exception e) when (e is KeyNotFoundException or InvalidOperationException
                                      or FormatException or ArgumentException)
        {
            //KeyNotFound — нет поля, InvalidOperation/Format — поле не того типа,
            //Argument — значение не прошло проверку в конструкторе модели
            return new LoadResult(label, false, e.Message);
        }
    }
}
