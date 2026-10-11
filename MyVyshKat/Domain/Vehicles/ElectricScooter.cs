namespace MyVyshKat.Domain.Vehicles;

//электросамокат
public class ElectricScooter : Vehicle, IEnergyConsumer
{
    public ElectricScooter(string id, string name, int simplicity, int wearPercentage, double dailyEnergyKwh)
        : base(id, name, simplicity, wearPercentage)
    {
        DailyEnergyKwh = EnergyValidation.EnsurePositive(dailyEnergyKwh);
    }

    public double DailyEnergyKwh { get; }
}
