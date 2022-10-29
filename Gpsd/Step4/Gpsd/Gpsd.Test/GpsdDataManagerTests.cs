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
        
        // Действие
        try
        {
            var result = _manager.ConvertToModel<GpsdDataModel>(data);
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

    /// <summary>
    /// Проверить получение корректного типа 
    /// </summary>
    [Test]
    [TestCase(@"{""class"":""VERSION"",""release"":""3.17"",""rev"":""3.17"",""proto_major"":3,""proto_minor"":12}")]
    public void Check_ConvertToModel_GpsdVersionModel(string data)
    {
        // Подготовка
        
        // Действие
        try
        {
            var result = _manager.ConvertToModel<GpsdVersionModel>(data);
            Assert.IsNotNull(result.Class);
            Assert.That(result.Release, Is.EqualTo("3.17"));
        }
        catch (InvalidDataException ex)
        {
            Console.WriteLine(ex);
            Assert.Fail();
        }
    }

    /// <summary>
    /// Проверить получение корректного типа 
    /// </summary>
    /// <param name="data"></param>
    [Test]
    [TestCase(@"{""class"":""SKY"",""device"":""/dev/pts/2"",""xdop"":0.47,""ydop"":0.57,""vdop"":0.72,""tdop"":0.68,""hdop"":0.73,""gdop"":1.46,""pdop"":1.03,""satellites"":[{""PRN"":18,""el"":60,""az"":200,""ss"":37,""used"":true},{""PRN"":29,""el"":59,""az"":106,""ss"":36,""used"":true},{""PRN"":26,""el"":54,""az"":271,""ss"":40,""used"":true},{""PRN"":5,""el"":41,""az"":78,""ss"":38,""used"":true},{""PRN"":16,""el"":34,""az"":303,""ss"":23,""used"":true},{""PRN"":20,""el"":23,""az"":43,""ss"":20,""used"":true},{""PRN"":31,""el"":14,""az"":233,""ss"":24,""used"":true},{""PRN"":9,""el"":4,""az"":338,""ss"":0,""used"":false},{""PRN"":27,""el"":1,""az"":279,""ss"":0,""used"":false},{""PRN"":136,""el"":0,""az"":0,""ss"":0,""used"":false},{""PRN"":88,""el"":61,""az"":62,""ss"":39,""used"":true},{""PRN"":81,""el"":60,""az"":258,""ss"":34,""used"":true},{""PRN"":79,""el"":55,""az"":246,""ss"":33,""used"":true},{""PRN"":72,""el"":30,""az"":44,""ss"":35,""used"":true},{""PRN"":65,""el"":22,""az"":98,""ss"":32,""used"":true},{""PRN"":78,""el"":20,""az"":192,""ss"":25,""used"":true},{""PRN"":87,""el"":14,""az"":67,""ss"":19,""used"":true},{""PRN"":71,""el"":13,""az"":356,""ss"":17,""used"":true},{""PRN"":82,""el"":11,""az"":251,""ss"":19,""used"":false}]}")]
    public void Check_ConvertToModel_GpsdSkyModel(string data)
    {
        // Подготовка
        
        // Действие
        try
        {
            var result = _manager.ConvertToModel<GpsdSkyModel>(data);
            Assert.IsNotNull(result.Class);
            Assert.IsTrue(result.Satellites.Any());
        }
        catch (InvalidDataException ex)
        {
            Console.WriteLine(ex);
            Assert.Fail();
        }
    }

    /// <summary>
    /// Проверить получение объекта типа <see cref="IGpsDataModel"/> сериализацией
    /// </summary>
    /// <param name="data"></param>
    [Test]
    [TestCase(@"{""class"":""SKY"",""device"":""/dev/pts/2"",""xdop"":0.47,""ydop"":0.57,""vdop"":0.72,""tdop"":0.68,""hdop"":0.73,""gdop"":1.46,""pdop"":1.03,""satellites"":[{""PRN"":18,""el"":60,""az"":200,""ss"":37,""used"":true},{""PRN"":29,""el"":59,""az"":106,""ss"":36,""used"":true},{""PRN"":26,""el"":54,""az"":271,""ss"":40,""used"":true},{""PRN"":5,""el"":41,""az"":78,""ss"":38,""used"":true},{""PRN"":16,""el"":34,""az"":303,""ss"":23,""used"":true},{""PRN"":20,""el"":23,""az"":43,""ss"":20,""used"":true},{""PRN"":31,""el"":14,""az"":233,""ss"":24,""used"":true},{""PRN"":9,""el"":4,""az"":338,""ss"":0,""used"":false},{""PRN"":27,""el"":1,""az"":279,""ss"":0,""used"":false},{""PRN"":136,""el"":0,""az"":0,""ss"":0,""used"":false},{""PRN"":88,""el"":61,""az"":62,""ss"":39,""used"":true},{""PRN"":81,""el"":60,""az"":258,""ss"":34,""used"":true},{""PRN"":79,""el"":55,""az"":246,""ss"":33,""used"":true},{""PRN"":72,""el"":30,""az"":44,""ss"":35,""used"":true},{""PRN"":65,""el"":22,""az"":98,""ss"":32,""used"":true},{""PRN"":78,""el"":20,""az"":192,""ss"":25,""used"":true},{""PRN"":87,""el"":14,""az"":67,""ss"":19,""used"":true},{""PRN"":71,""el"":13,""az"":356,""ss"":17,""used"":true},{""PRN"":82,""el"":11,""az"":251,""ss"":19,""used"":false}]}")]
    public void Check_Get_GpsdSkyModel(string data)
    {
        // Подготовка
        
        // Действие
        var result = _manager.CreateModel(data);
        
        // Проверки
        Assert.IsNotNull(result);
        Assert.IsNotNull(result as GpsdSkyModel);
    }
}