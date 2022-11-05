using Gpsd.Core.Models;

namespace Gpsd.Core;

/// <summary>
/// Интерфейс - фабрика 
/// </summary>
public interface IGpsdDataFactory
{
    /// <summary>
    /// Создать модель
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public IGpsDataModel? CreateModel(DataModelType type);

    /// <summary>
    /// Определить тип модели
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public Type GetTypeOfModel(DataModelType type);
}