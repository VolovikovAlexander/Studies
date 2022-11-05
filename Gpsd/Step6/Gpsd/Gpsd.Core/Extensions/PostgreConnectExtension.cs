using System.Data;
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
}