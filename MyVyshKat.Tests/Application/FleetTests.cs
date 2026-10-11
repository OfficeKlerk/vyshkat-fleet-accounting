using MyVyshKat.Application;
using MyVyshKat.Domain;
using MyVyshKat.Tests.Fakes;

namespace MyVyshKat.Tests.Application;

//поведение парка при разных решениях техосмотра;
//настоящий ServiceCenter подменён FakeServiceCenter через конструктор
public class FleetTests
{
    [Fact]
    public void AddItem_WhenInspectionPasses_AddsItem()
    {
        var serviceCenter = FakeServiceCenter.AcceptAll();
        var fleet = new Fleet(serviceCenter);
        var scooter = Samples.Scooter();

        var result = fleet.AddItem(scooter);

        Assert.True(result.Accepted);
        Assert.Same(scooter, Assert.Single(fleet.GetItems()));
        Assert.Same(scooter, Assert.Single(serviceCenter.Inspected));
    }

    [Fact]
    public void AddItem_WhenInspectionFails_DoesNotAdd()
    {
        var fleet = new Fleet(FakeServiceCenter.RejectAll("Brakes are broken"));

        var result = fleet.AddItem(Samples.Scooter());

        Assert.False(result.Accepted);
        Assert.Equal("Brakes are broken", result.RejectionReason);
        Assert.Empty(fleet.GetItems());
    }

    [Fact]
    public void AddItem_DuplicateId_IsRejectedWithoutInspection()
    {
        var serviceCenter = FakeServiceCenter.AcceptAll();
        var fleet = new Fleet(serviceCenter);
        fleet.AddItem(Samples.Scooter(id: "VK-1"));

        var result = fleet.AddItem(Samples.Helmet(id: "VK-1"));

        Assert.False(result.Accepted);
        Assert.Contains("VK-1", result.RejectionReason);
        Assert.Single(serviceCenter.Inspected);
        Assert.Single(fleet.GetItems());
    }

    [Fact]
    public void GetItems_CannotBeModifiedFromOutside()
    {
        var fleet = new Fleet(FakeServiceCenter.AcceptAll());
        var items = (IList<InventoryItem>)fleet.GetItems();

        Assert.Throws<NotSupportedException>(() => items.Add(Samples.Helmet()));
        Assert.Empty(fleet.GetItems());
    }
}
