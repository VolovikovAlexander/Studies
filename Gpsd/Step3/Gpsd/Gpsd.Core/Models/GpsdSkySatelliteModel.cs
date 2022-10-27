using System.Text.Json.Serialization;

namespace Gpsd.Core.Models;

/// <summary>
/// Класс модель - описание спутника
/// :[{"PRN":18,"el":61,"az":200,"ss":0,"used":false}
/// </summary>
public class GpsdSkySatelliteModel
{
    [JsonPropertyName("PRN")]
    public int Number { get; set; }
    
    [JsonPropertyName("az")]
    public double Azimuth { get; set; }
    
    [JsonPropertyName("used")]
    public bool Used { get; set; }
}