namespace MyVyshKat.Domain.Things;

public abstract class Thing : InventoryItem
{
    protected Thing(string id, string name) : base(id, name){}
}