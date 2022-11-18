using System.Text.Json.Serialization;

namespace Gpsd.Core.Models;

/// <summary>
/// Модель описание - VERSION
/// {"class":"VERSION","release":"3.17","rev":"3.17","proto_major":3,"proto_minor":12}
/// </summary>
[TableName("data_version")]
public class GpsdVersionModel: GpsdDataModel
{
    [JsonPropertyName("release")]
    public string Release { get; set; } = String.Empty;

    [JsonPropertyName("rev")]
    public string Revision { get; set; } = string.Empty;

    [JsonPropertyName("proto_major")]
    public int Major { get; set; }

    [JsonPropertyName("proto_minor")]
    public int Minor { get; set; }

    [JsonPropertyName("type_id")]
    public override long DataType => 2;
}