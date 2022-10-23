using System.Text.Json.Serialization;

namespace Gpsd.Core.Models;

/// <summary>
/// Общий интерфейс к системе парсинга данных от Gpsd сервиса
/// </summary>
public interface IGpsDataModel
{
    public string Class { get; set; } 
}