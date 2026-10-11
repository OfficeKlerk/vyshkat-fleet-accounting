namespace MyVyshKat.Domain.Vehicles;

//электровелосипед
public class ElectricBike : Vehicle, IEnergyConsumer
{
    public ElectricBike(string id, string name, int simplicity, int wearPercentage, double dailyEnergyKwh)
        : base(id, name, simplicity, wearPercentage)
    {
        DailyEnergyKwh = EnergyValidation.EnsurePositive(dailyEnergyKwh);
    }

    public double DailyEnergyKwh { get; }
}
