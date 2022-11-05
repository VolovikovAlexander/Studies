using System.Text.Json.Serialization;

namespace Gpsd.Core.Models;

/// <summary>
/// Описание модели - SKY
/// {"class":"SKY","device":"/dev/pts/2","vdop":4.05,"hdop":5.56,"pdop":6.88,"satellites":[{"PRN":18,"el":61,"az":200,"ss":0,"used":false},{"PRN":29,"el":58,"az":106,"ss":0,"used":false},{"PRN":26,"el":54,"az":269,"ss":0,"used":false},{"PRN":5,"el":41,"az":76,"ss":0,"used":false},{"PRN":16,"el":35,"az":302,"ss":0,"used":false},{"PRN":20,"el":23,"az":43,"ss":0,"used":false},{"PRN":31,"el":12,"az":232,"ss":0,"used":false},{"PRN":9,"el":4,"az":337,"ss":0,"used":false},{"PRN":27,"el":2,"az":280,"ss":0,"used":false},{"PRN":81,"el":62,"az":260,"ss":0,"used":false},{"PRN":88,"el":59,"az":63,"ss":0,"used":false},{"PRN":79,"el":53,"az":243,"ss":0,"used":false},{"PRN":72,"el":30,"az":41,"ss":0,"used":false},{"PRN":65,"el":23,"az":97,"ss":0,"used":false},{"PRN":78,"el":18,"az":192,"ss":0,"used":false},{"PRN":87,"el":13,"az":68,"ss":0,"used":false},{"PRN":82,"el":12,"az":252,"ss":0,"used":false},{"PRN":71,"el":11,"az":355,"ss":0,"used":false}]}
/// </summary>
[TableName("data_sky")]
public class GpsdSkyModel: GpsdDataModel
{
    [JsonPropertyName("device")]
    public string Device { get; set; } = string.Empty;
    
    [JsonPropertyName("satellites")]
    public IEnumerable<GpsdSkySatelliteModel>? Satellites { get; set; }

    /*
     *  HDOP – horizontal dilution of precision
        VDOP – vertical dilution of precision
        PDOP – position (3D) dilution of precision
        TDOP – time dilution of precision
        GDOP – geometric dilution of precision
     */
    
    [JsonPropertyName("hdop")]
    public double HorizontalPrecision { get; set; }
    
    [JsonPropertyName("vdop")]
    public double VerticalPrecision { get; set; }
    
    [JsonPropertyName("pdop")]
    public double PositionPrecision { get; set; }
}