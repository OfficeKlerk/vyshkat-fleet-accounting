using MyVyshKat.Domain.Vehicles;

namespace MyVyshKat.Application;

public class SimplicityRule : IBeginnerRule
{
    private readonly int _minSimplicity;
    public string Description { get; }
    
    public SimplicityRule(int minSimplicity)
    {
        _minSimplicity = minSimplicity;
        Description = $"Simplicity >= {minSimplicity}";
    }
    
    public bool IsSuitable(Vehicle vehicle)
    {
        return vehicle.Simplicity >= _minSimplicity;
    }
    
}