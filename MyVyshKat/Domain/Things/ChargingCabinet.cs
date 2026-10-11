namespace MyVyshKat.Domain.Things;

public class ChargingCabinet : Thing, IEnergyConsumer
{
    public ChargingCabinet(string id, string name, double dailyEnergyKwh) : base(id, name)
    {
        DailyEnergyKwh = EnergyValidation.EnsurePositive(dailyEnergyKwh);
    }
    public double DailyEnergyKwh { get; }
}