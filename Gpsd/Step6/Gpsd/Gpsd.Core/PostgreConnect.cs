using System.Data;

namespace Gpsd.Core;

/// <summary>
/// Класс - описание настроек подключение к базе данных PostgreSql
/// </summary>
public class PostgreConnect
{
    
    #region Свойства

    /// <summary>
    /// Хост
    /// </summary>
    public string Host { get; set; } = "localhost";

    /// <summary>
    /// Порт подключения
    /// </summary>
    public long Port { get; set; } = 5432;

    /// <summary>
    /// Имя пользователя
    /// </summary>
    public string Login { get; set; } = "gpsd";

    /// <summary>
    /// Пароль
    /// </summary>
    public string Password { get; set; } = "123456";

    /// <summary>
    /// Наименование базы данных
    /// </summary>
    public string Database { get; set; } = "gpsd";

    #endregion

    public override string ToString()
        => string.Format(
            "User ID={0};Password={1};Host={2};Port={3};Database={4};Pooling=true;",
            Login, Password, Host, Port, Database);
}