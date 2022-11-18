using System.Data;
using System.Reflection;
using System.Text.Json.Serialization;
using Gpsd.Core.Models;
using Npgsql;

namespace Gpsd.Core;

public static class PostgreConnectExtension
{
    /// <summary>
    /// Получить выборку данных из PostgreSql
    /// </summary>
    /// <param name="connect"> Информация о соединении </param>
    /// <param name="sql"> SQL запрос </param>
    /// <param name="parameters"> Набор параметров </param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public static DataTable GetTable(this PostgreConnect connect, string sql, Dictionary<string, object>? parameters = null)
    {
        ArgumentNullException.ThrowIfNull(connect);
        if (string.IsNullOrEmpty(sql))
            throw new ArgumentException("Не указан SQL запрос для выполнения выборки данных");

        try
        {
            using var database = new NpgsqlConnection(connect.ToString());
            database.Open();
            var command = new NpgsqlCommand(sql, database);
            command.Parameters.AddRange(parameters.GetPostgreParameters().ToArray());

            using var reader = command.ExecuteReader(CommandBehavior.CloseConnection);
            var result = new DataTable();
            result.Load(reader);
            return result;
        }  
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                $"Ошибка при выполнении Sql запроса! \n{sql}. Соединение: {connect.ToString()}", ex);
        }  
    }

    /// <summary>
    /// Сформировать набор параметров для SQL запроса
    /// </summary>
    /// <param name="parameters"> Исходный словарь с параметрами </param>
    /// <returns></returns>
    private static IEnumerable<NpgsqlParameter> GetPostgreParameters(this Dictionary<string, object>? parameters)
    {
        if (parameters is null) return Enumerable.Empty<NpgsqlParameter>();
        var result = parameters.Select(x => new NpgsqlParameter
        {
            ParameterName = x.Key, Value = x.Value
        });
        return result;
    }

    /// <summary>
    /// Вставить данные в таблицу из модели <see cref="IGpsDataModel"/>
    /// </summary>
    /// <param name="connect"></param>
    /// <param name="model"> Исходная модель </param>
    public static long InsertData(this PostgreConnect connect, IGpsDataModel model)
    {
        ArgumentNullException.ThrowIfNull(connect);
        ArgumentNullException.ThrowIfNull(model);

        var tableAttributes = model.GetType().GetCustomAttribute(typeof(TableNameAttribute))  as TableNameAttribute;
        if (tableAttributes is null)
            throw new InvalidCastException();

        // Наименование таблицы
        var tableName = tableAttributes.TableName;
        
        // Собираем поля для вставвки
        var fields = model.GetType().GetProperties()
            .Select(x => new
            {
                Name = x.Name,
                JsonName = x.GetCustomAttribute(typeof(JsonPropertyNameAttribute)) as JsonPropertyNameAttribute,
                Value = x.GetValue(model)
            }).Where(x => x.JsonName is not null) .ToList();

        var fieldsNames = fields.Select(x => x.JsonName?.Name ?? "").ToArray();
        var fieldsValues = fields.Select(x => $"'{x.Value}'").ToArray();
        var sql =
            $"insert into {tableName} ({string.Join(",", fieldsNames)}) values({string.Join(",", fieldsValues)});";
        
        // Выполним запрос и получим код вставки
        try
        {
            using var database = new NpgsqlConnection(connect.ToString());
            database.Open();
            var command = new NpgsqlCommand(sql, database);

            var result = command.ExecuteNonQuery();
            return result;
        }  
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                $"Ошибка при выполнении Sql запроса! \n{sql}. Соединение: {connect.ToString()}", ex);
        }  
    }
}