using System.Text.Json;
using Gpsd.Core.Models;

namespace Gpsd.Core;

/// <summary>
/// Основной класс для работы с данными от GPSD сервиса 
/// </summary>
public class GpsdDataManager
{
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
}