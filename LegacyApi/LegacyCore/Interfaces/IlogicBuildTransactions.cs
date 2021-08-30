using System.Linq;
using LegacyCore.Models;

namespace LegacyCore.Interfaces
{
    /// <summary>
    /// Интерфейс для реализации алгоритма обработки данных
    /// </summary>
    public interface IlogicBuildTransactions
    {
        /// <summary>
        /// Удалить подготовденные данные
        /// </summary>
        /// <returns></returns>
        public bool RemoveRecord(ITransactionsPeriod period);

        /// <summary>
        /// Получить выборку данных из таблицы фактов
        /// </summary>
        /// <param name="period"></param>
        /// <returns></returns>
        public IQueryable<TblTransactionFact> GetTransactions(legacyContext context, ITransactionsPeriod period);

        /// <summary>
        /// Получить и добавить новые счета
        /// </summary>
        /// <param name="source"></param>
        /// <param name="period"></param>
        /// <returns></returns>
        public bool AddAccounts(ITransactionsPeriod period);

        /// <summary>
        /// Получить и добавить новых клиентов
        /// </summary>
        /// <param name="source"></param>
        /// <param name="period"></param>
        /// <returns></returns>
        public bool AddCustomers( ITransactionsPeriod period);

        /// <summary>
        /// Получить и добавить новые договора
        /// </summary>
        /// <param name="source"></param>
        /// <param name="period"></param>
        /// <returns></returns>
        public bool AddContracts( ITransactionsPeriod period);

        /// <summary>
        /// Добавить обработанные транзакции
        /// </summary>
        /// <param name="source"></param>
        /// <param name="period"></param>
        /// <returns></returns>
        public bool AddTransactions(ITransactionsPeriod period);
    }
}
