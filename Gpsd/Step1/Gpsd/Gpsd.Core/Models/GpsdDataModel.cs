using System.Text.Json;
using System.Text.Json.Serialization;

namespace Gpsd.Core.Models;

/// <summary>
/// Общий класс для создания серилиазационных моделей 
/// </summary>
public class GpsdDataModel: IGpsDataModel
{
    private string _class = "";
    private DataModelType _modelType = DataModelType.NONAME;
    
    /// <summary>
    /// Наименование класса модели
    /// </summary>
    [JsonPropertyName("class")]
    public string Class { get => _class;
        set
        {
            _class = value ?? throw new ArgumentNullException(nameof(Class));
            Enum.TryParse<DataModelType>(_class, true, out _modelType);
        }
    }

    /// <summary>
    /// Тип класса модели
    /// </summary>
    [JsonIgnore]
    public DataModelType ModelType => _modelType;
}