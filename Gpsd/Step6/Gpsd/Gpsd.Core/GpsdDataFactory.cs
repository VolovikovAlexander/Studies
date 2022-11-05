using System.ComponentModel;
using Gpsd.Core.Models;

namespace Gpsd.Core;

/// <summary>
/// Реализация интерфейса <see cref="IGpsdDataFactory"/>
/// </summary>
public class GpsdDataFactory: IGpsdDataFactory
{
    // Карта связей
    private IDictionary<DataModelType, Type> _templates = new Dictionary<DataModelType, Type>()
    {
        { DataModelType.SKY, typeof(GpsdSkyModel) },
        { DataModelType.VERSION, typeof(GpsdVersionModel) }
    };

    /// <summary>
    /// Создать объект треебуемого типа
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public IGpsDataModel? CreateModel(DataModelType type)
        => Activator.CreateInstance(this.GetTypeOfModel(type)) as IGpsDataModel;
    
    /// <summary>
    /// Получить требуемый тип данных по значению перечисления <see cref="DataModelType"/>
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    /// <exception cref="InvalidEnumArgumentException"></exception>
    public Type GetTypeOfModel(DataModelType type)
        => _templates.ContainsKey(type)
            ? _templates[type]
            : throw new InvalidEnumArgumentException(
                "Некорректно переданы параметры! Для указанного перечисления нет реализации!");

}