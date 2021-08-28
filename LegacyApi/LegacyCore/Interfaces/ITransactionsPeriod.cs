using System;

namespace LegacyCore.Interfaces
{
    /// <summary>
    /// Интерфейс для определения блока периодов из исходной таблицы с транзакциями
    /// </summary>
    public interface ITransactionsPeriod
    {
        public DateTime StartPeriod { get; set; }

        public DateTime StopPeriod { get; set; }
    }
}
