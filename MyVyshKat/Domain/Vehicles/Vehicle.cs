namespace MyVyshKat.Domain.Vehicles;

public abstract class Vehicle : InventoryItem
{
    protected Vehicle(string id, string name, int simplicity, int wearPercentage) : base(id, name)
    {
        if (simplicity < 1 || simplicity > 10)
        {
            throw new ArgumentOutOfRangeException(nameof(simplicity), simplicity, 
                                                "Simplicity must be between 1 and 10");
        }
        if (wearPercentage < 0 || wearPercentage > 100)
        {
            throw new ArgumentOutOfRangeException(nameof(wearPercentage), wearPercentage, 
                                                "Wear percentage must be between 0 and 100");
        }
        
        Simplicity = simplicity;
        WearPercentage = wearPercentage;
    }

    public int Simplicity { get; }

    public int WearPercentage { get; }
}