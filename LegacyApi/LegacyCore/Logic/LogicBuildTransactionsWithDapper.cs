using LegacyCore.Interfaces;
using Dapper;
using Microsoft.Data.SqlClient;

namespace LegacyCore.Logic
{
    public class LogicBuildTransactionsWithDapper: LogicBuildTransactions
    {
        public override bool RemoveRecord(ITransactionsPeriod period)
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
