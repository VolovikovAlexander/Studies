namespace Gpsd.Core.Models;

/// <summary>
/// Специальный интерфейс для связывания произвольной модели в базой данных
/// </summary>
public interface IDataEntity
{
    public long Id { get; set; }
}