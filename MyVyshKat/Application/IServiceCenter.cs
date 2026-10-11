using MyVyshKat.Domain;

namespace MyVyshKat.Application;

public interface IServiceCenter
{
    InspectionResult ApplyItem(InventoryItem item);
}