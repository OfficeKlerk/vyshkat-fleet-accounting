using MyVyshKat.Domain;
using MyVyshKat.Domain.Vehicles;

namespace MyVyshKat.Application;

public interface IBeginnerRule
{
    string Description { get; }
    bool IsSuitable(Vehicle item);
}