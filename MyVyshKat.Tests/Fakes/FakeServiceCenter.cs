using MyVyshKat.Application;
using MyVyshKat.Domain;

namespace MyVyshKat.Tests.Fakes;

//подменённый техосмотр: всегда отвечает заранее заданным решением
//и запоминает, какие предметы ему показывали (stub + spy)
public class FakeServiceCenter : IServiceCenter
{
    private readonly InspectionResult _result;

    public FakeServiceCenter(InspectionResult result)
    {
        _result = result;
    }

    public List<InventoryItem> Inspected { get; } = new List<InventoryItem>();

    public static FakeServiceCenter AcceptAll()
    {
        return new FakeServiceCenter(new InspectionResult(true));
    }

    public static FakeServiceCenter RejectAll(string reason = "Rejected by fake")
    {
        return new FakeServiceCenter(new InspectionResult(false, reason));
    }

    public InspectionResult ApplyItem(InventoryItem item)
    {
        Inspected.Add(item);
        return _result;
    }
}
