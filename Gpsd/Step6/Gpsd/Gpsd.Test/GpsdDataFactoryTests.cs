using System.ComponentModel;
using Gpsd.Core;
using Gpsd.Core.Models;

namespace Gpsd.Test;

public class GpsdDataFactoryTests
{
    /// <summary>
    /// Проверить получение типа по перечислению. Верные данные.
    /// </summary>
    /// <param name="type"></param>
    [Test]
    [TestCase(DataModelType.SKY, typeof(GpsdSkyModel))]
    [TestCase(DataModelType.VERSION, typeof(GpsdVersionModel))]
    public void Check_GetType_Try(DataModelType typeOfModel, Type typeAssert)
    {
        // Подготовка
        var factory = new GpsdDataFactory();
        
        // Действие
        var result = factory.GetTypeOfModel(typeOfModel);
        
        // Проверки
        Assert.IsNotNull(result);
        Assert.That(typeAssert, Is.EqualTo(result));
    }

    /// <summary>
    /// Проверка получение типа по перечислению. Неверные данные.
    /// </summary>
    /// <param name="typeOfModel"></param>
    [Test]
    [TestCase(DataModelType.WATCH)]
    public void Check_GetType_Fail(DataModelType typeOfModel)
    {
        // Подготовка
        var factory = new GpsdDataFactory();

        try
        {
            // Действие
            var result = factory.GetTypeOfModel(typeOfModel);
            Assert.Fail("При выполнении данной операции должно быть исключение типа InvalidEnumArgumentException!");
        }
        catch (InvalidEnumArgumentException)
        {
            Assert.Pass();
        }
    }
}