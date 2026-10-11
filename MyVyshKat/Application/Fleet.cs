using System.Collections.ObjectModel;
using MyVyshKat.Domain;

namespace MyVyshKat.Application;

public class Fleet
{
    private readonly List<InventoryItem> _items;
    private readonly IServiceCenter _serviceCenter;

    public Fleet(IServiceCenter serviceCenter)
    {
        _serviceCenter = serviceCenter;
        _items = new List<InventoryItem>();
    }

    public InspectionResult AddItem(InventoryItem item)
    {
        if (_items.Any(i => i.Id == item.Id))
        {
            return new InspectionResult(false, $"Inventory number {item.Id} is already taken");
        }
        
        InspectionResult result = _serviceCenter.ApplyItem(item);
        if (result.Accepted)
        {
            _items.Add(item);
        }
        
        return result;
    }

    public ReadOnlyCollection<InventoryItem> GetItems()
    {
        return _items.AsReadOnly();
    }
    
}