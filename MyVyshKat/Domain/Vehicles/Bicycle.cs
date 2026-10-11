namespace MyVyshKat.Domain.Vehicles;

//обычный велосипед
public class Bicycle : Vehicle
{
    public Bicycle(string id, string name, int simplicity, int wearPercentage)
        : base(id, name, simplicity, wearPercentage)
    {
    }
}
