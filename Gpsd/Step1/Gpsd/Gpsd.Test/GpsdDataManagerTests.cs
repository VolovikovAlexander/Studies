using Gpsd.Core;
using Gpsd.Core.Models;

namespace Gpsd.Test;

public class GpsdDataManagerTests
{
    private GpsdDataManager _manager;
    [SetUp]
    public void Setup()
    {
        _manager = new GpsdDataManager();
    }

    [Test]
    [TestCase(@"{""class"":""VERSION"",""release"":""3.17"",""rev"":""3.17"",""proto_major"":3,""proto_minor"":12}")]
    [TestCase(@"{""class"":""TPV"",""device"":""/dev/ttyS3"",""mode"":1,""time"":""1980-01-06T00:22:47.868Z"",""ept"":0.005}")]
    public void Check_ConvertToModel(string data)
    {
        // Подготовка
        
        // Дейтсвие
        try
        {
            var result = _manager.ConvertToModel(data);
            Assert.IsNotNull(result.Class);

            var item = result as GpsdDataModel;
            Assert.IsNotNull(item);
            Assert.That(item?.ModelType.ToString(), Is.EqualTo(item?.Class ?? ""));
        }
        catch (InvalidDataException ex)
        {
            Console.WriteLine(ex);
            Assert.Fail();
        }
    }
}