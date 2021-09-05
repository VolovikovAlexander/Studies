using LegacyCore.Interfaces;
using Dapper;
using Microsoft.Data.SqlClient;

namespace LegacyCore.Logic
{
    /// <summary>
    /// Класс наследник от <see cref="LogicBuildTransactions"/> для переопределения операции удаления данных.
    /// </summary>
    public class LogicBuildTransactionsWithDapper: LogicBuildTransactions
    {
        /// <summary>
        /// Удалить старые данные перед вставкой новых записей
        /// </summary>
        /// <param name="period"></param>
        /// <returns></returns>
        public override bool RemoveRecords(ITransactionsPeriod period)
        {
            var dbPeriod = GetPeriod(period);

            var cs = @"Server=LEGACYSERVER\SQLEXPRESS;Database=LegacyFinish;Trusted_Connection=No;Connection Timeout=500;User Id=sa;Password=123456;";
            using var connect = new SqlConnection(cs);
            connect.Open();

            var sql = $"Delete from tblTransactions where PeriodId = {dbPeriod.Id}";
            var version = connect.ExecuteScalar(sql);
            return true;
        }
    }
}
