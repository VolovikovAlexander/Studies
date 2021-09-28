using System.Collections.Generic;

namespace LegacyCore.Interfaces
{
    /// <summary>
    /// Интерфейс для взаимодействия с исходными транзакциями
    /// </summary>
    public interface ITransactions
    {
        /// <summary>
        /// Флаг. Есть новые данные для обработки
        /// </summary>
        public bool IsSuccess { get; }

        /// <summary>
        /// Получить список периодов из порции новых записей с разбивкой по месяцу.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ITransactionsPeriod> GetPeriods();

        /// <summary>
        /// Получить для обработки блок данных
        /// </summary>
        /// <param name="period"></param>
        /// <returns></returns>
        public IEnumerable<IModel> GetNextRecords(ITransactionsPeriod period);
    }
}
