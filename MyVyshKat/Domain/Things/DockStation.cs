namespace MyVyshKat.Domain.Things;

public class DockStation : Thing, IEnergyConsumer
{
    public DockStation(string id, string name, double dailyEnergyKwh) : base(id, name)
    {
        DailyEnergyKwh = EnergyValidation.EnsurePositive(dailyEnergyKwh);
    }
    public double DailyEnergyKwh { get; }
}