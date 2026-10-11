namespace MyVyshKat.Domain;

public abstract class InventoryItem : IInventarizable
{
    protected InventoryItem(string id, string name)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            throw new ArgumentException("Inventory number cannot be empty", nameof(id));
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name cannot be empty", nameof(name));
        }
        
        Id = id;
        Name = name;
    }
    
    public string Id { get; }
    public string Name { get; }
}