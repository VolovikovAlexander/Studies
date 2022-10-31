using Gpsd.Core;
using Gpsd.Core.Extensions;

namespace Gpsd.Test;

public class PostgreTests
{
    /// <summary>
    /// Проверить подключение и выборку данных
    /// </summary>
    [Test]
    public void Check_Connect_DataBase()
    {
        // Подготовка
        var connect = new PostgreConnect();
        var sql = "select * from data_types";

        // Действие
        var result = connect.GetTable(sql);

        // Проверки
        Assert.IsNotNull(result);
    }

    /// <summary>
    /// Проверить подключение и выборку данных с использованием параметров
    /// </summary>
    [Test]
    public void Check_Connect_Database_WithParameters()
    {
        // Подготовка
        var connect = new PostgreConnect();
        var sql = "select * from data_types where id = @Id" ;
        var parameters = new Dictionary<string, object?>()
        {
            { "Id", 1 }
        };

        // Действие
        var result = connect.GetTable(sql, parameters!);

        // Проверки
        Assert.IsNotNull(result);
        Assert.That(result.Rows.Count, Is.EqualTo(1));
    }
}