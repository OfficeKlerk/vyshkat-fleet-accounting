using MyVyshKat.Domain.Things;
using MyVyshKat.Domain.Vehicles;

namespace MyVyshKat.Tests.Fakes;

//быстрое создание тестовых объектов с разумными значениями по умолчанию
public static class Samples
{
    public static ElectricScooter Scooter(string id = "S-1", int simplicity = 7, int wear = 10, double energy = 0.8)
    {
        return new ElectricScooter(id, $"Scooter {id}", simplicity, wear, energy);
    }

    public static ElectricBike EBike(string id = "E-1", int simplicity = 5, int wear = 10, double energy = 1.1)
    {
        return new ElectricBike(id, $"EBike {id}", simplicity, wear, energy);
    }

    public static Bicycle Bicycle(string id = "B-1", int simplicity = 9, int wear = 10)
    {
        return new Bicycle(id, $"Bicycle {id}", simplicity, wear);
    }

    public static Helmet Helmet(string id = "H-1")
    {
        return new Helmet(id, $"Helmet {id}");
    }

    public static DockStation Dock(string id = "D-1", double energy = 3.5)
    {
        return new DockStation(id, $"Dock {id}", energy);
    }

    public static ChargingCabinet Cabinet(string id = "C-1", double energy = 2.0)
    {
        return new ChargingCabinet(id, $"Cabinet {id}", energy);
    }
}
