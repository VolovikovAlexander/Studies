namespace Gpsd.Core;

/// <summary>
/// Класс атрибут для хранения информации о наименовании таблицы базы данных
/// </summary>
public class TableNameAttribute: Attribute
{
    private string _tableName = string.Empty;

    public TableNameAttribute(string tableName)
    {
        if (string.IsNullOrEmpty(tableName))
            throw new ArgumentException("Некорректно переданы параметры!", nameof(tableName));
        
        _tableName = tableName;
    }

    /// <summary>
    /// Наименование связанной таблице в базе данных
    /// </summary>
    public string TableName => _tableName;
}