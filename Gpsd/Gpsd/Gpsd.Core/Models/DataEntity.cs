using System.Text.Json.Serialization;

namespace Gpsd.Core.Models;

/// <summary>
/// Абстрактный класс для наследования. Указывает уникальный код записи в базе данных
/// </summary>
public abstract class DataEntity: IDataEntity
{
    [JsonIgnore]
    public long Id { get; set; }
}