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
    public IGpsDataModel ConvertToModel(string data)
    {
        if (string.IsNullOrEmpty(data))
            throw new ArgumentNullException(nameof(data));

        var result = JsonSerializer.Deserialize<GpsdDataModel>(data)
                     ?? throw new InvalidDataException(
                         "Невозможно сконвертировать входные данные в тип IGpsDataModel!");
        return result;
    }
}