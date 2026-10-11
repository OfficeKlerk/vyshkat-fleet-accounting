using System.Globalization;
using System.Text.Json;
using MyVyshKat.Application;
using MyVyshKat.Domain.Vehicles;
using MyVyshKat.Infrastructure.Files;

namespace MyVyshKat.Infrastructure.ConsoleUi;

//интерактивное меню
public class ConsoleMenu
{
    private readonly FleetReports _reports;
    private readonly JsonInventoryLoader _loader;

    //абстракция потоков
    private readonly TextReader _input;
    private readonly TextWriter _output;

    public ConsoleMenu(FleetReports reports, JsonInventoryLoader loader, TextReader input, TextWriter output)
    {
        _reports = reports;
        _loader = loader;
        _input = input;
        _output = output;
    }

    //главный цикл
    public void Run()
    {
        _output.WriteLine("VyshKat fleet accounting");

        while (true)
        {
            PrintMenu();
            var choice = ReadLine("Choose an option");

            //конец ввода (Ctrl+D или конец файла при запуске через <) — выходим штатно
            if (choice is null || choice == "0")
            {
                _output.WriteLine("Bye!");
                return;
            }

            _output.WriteLine();
            switch (choice)
            {
                case "1":
                    LoadFromFile();
                    break;
                case "2":
                    PrintSummary();
                    break;
                case "3":
                    PrintBeginnerVehicles();
                    break;
                case "4":
                    PrintInventory();
                    break;
                default:
                    //неверный пункт — подсказываем и показываем меню снова, программа не падает
                    _output.WriteLine("Unknown option, enter a number from 0 to 4");
                    break;
            }

            _output.WriteLine();
        }
    }

    private void PrintMenu()
    {
        _output.WriteLine("1. Accept items from a JSON file");
        _output.WriteLine("2. Fleet summary: units and total daily energy");
        _output.WriteLine("3. Vehicles for beginners");
        _output.WriteLine("4. Inventory list");
        _output.WriteLine("0. Exit");
    }

    //приём предметов из файла
    private void LoadFromFile()
    {
        var path = ReadLine("Path to JSON file");
        if (string.IsNullOrWhiteSpace(path))
        {
            _output.WriteLine("Path is empty, nothing loaded");
            return;
        }

        //ошибки всего файла (нет файла, нет доступа, битый json, не массив) ловим здесь,
        //ошибки отдельных предметов загрузчик уже вернул в results
        List<LoadResult> results;
        try
        {
            results = _loader.LoadFromFile(path);
        }
        catch (Exception e) when (e is IOException or UnauthorizedAccessException
                                      or JsonException or InvalidDataException)
        {
            _output.WriteLine($"Cannot load file: {e.Message}");
            return;
        }

        //по каждому предмету: принят или нет, и если нет — почему
        foreach (var result in results)
        {
            _output.WriteLine(result.Accepted
                ? $"  [accepted] {result.Item}"
                : $"  [rejected] {result.Item}: {result.Reason}");
        }

        var accepted = results.Count(result => result.Accepted);
        _output.WriteLine($"Accepted {accepted} of {results.Count}");
    }

    //число единиц на балансе и суммарный суточный расход энергии
    private void PrintSummary()
    {
        _output.WriteLine($"Units on balance: {_reports.GetItemsQuantity()}");
        _output.WriteLine($"Total daily energy consumption: {FormatKwh(_reports.GetCommonEnergyConsumption())}");
    }

    //техника для новичков; в заголовке - правило, по которому её отобрали
    private void PrintBeginnerVehicles()
    {
        var items = _reports.GetBeginnersItems();
        _output.WriteLine($"Vehicles for beginners ({_reports.BeginnerRuleDescription}):");
        if (items.Count == 0)
        {
            _output.WriteLine("  none");
            return;
        }

        foreach (var item in items)
        {
            var vehicle = (Vehicle)item;
            _output.WriteLine($"  {vehicle.Id}  {vehicle.Name}  simplicity {vehicle.Simplicity}/10");
        }
    }

    //инвентарная ведомость
    private void PrintInventory()
    {
        var entries = _reports.GetReport();
        _output.WriteLine("Inventory list:");
        if (entries.Count == 0)
        {
            _output.WriteLine("  nothing on balance");
            return;
        }

        foreach (var entry in entries)
        {
            _output.WriteLine($"  {entry.Id}  {entry.Name}");
        }
    }

    //null означает, что ввод закончился
    private string? ReadLine(string prompt)
    {
        _output.Write($"{prompt}: ");
        return _input.ReadLine()?.Trim();
    }

    //всегда с точкой и не больше двух знаков после неё
    private static string FormatKwh(double value)
    {
        return value.ToString("0.##", CultureInfo.InvariantCulture) + " kWh";
    }
}
