using System.Text.Json;
using Gpsd.Core.Models;

namespace Gpsd.Core;

/// <summary>
/// Основной класс для работы с данными от GPSD сервиса 
/// </summary>
public class GpsdDataManager
{
    private readonly GpsdDataFactory _factory = new GpsdDataFactory();
    
    /// <summary>
    /// Сконвертировать строковые данные в модель типа <see cref="IGpsDataModel"/>
    /// </summary>
    /// <param name="data"> Строковые данные </param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public T ConvertToModel<T>(string data) where T : class, IGpsDataModel
    {
        if (string.IsNullOrEmpty(data))
            throw new ArgumentNullException(nameof(data));

        var result = JsonSerializer.Deserialize<T>(data)
                     ?? throw new InvalidDataException(
                         "Невозможно сконвертировать входные данные в тип IGpsDataModel!");
        return result;
    }

    /// <summary>
    /// Получить объект типа <see cref="IGpsDataModel"/> из Json строки
    /// </summary>
    /// <param name="data"> Строка в формате Json </param>
    /// <returns></returns>
    public IGpsDataModel CreateModel(string data)
    {
        if (string.IsNullOrEmpty(data))
            throw new ArgumentException( nameof(data));

        var baseObject = ConvertToModel<GpsdDataModel>(data);
        var gpsdType = _factory.GetTypeOfModel(baseObject.ModelType);
        var result = JsonSerializer.Deserialize(data, gpsdType) as IGpsDataModel;
        return result ?? throw new InvalidOperationException("Невозможно получить объект по указанной строке!");
    }
}