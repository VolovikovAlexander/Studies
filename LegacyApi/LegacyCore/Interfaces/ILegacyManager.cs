using System.Collections.Generic;

namespace LegacyCore.Interfaces
{
    // <summary>
    /// Интерфейс к бизнес системе обработки новых данных
    /// </summary>
    public interface ILegacyManager
    {
        /// <summary>
        /// Объект для работы с новыми данными
        /// </summary>
        public ITransactions transactions { get; set; }

        /// <summary>
        /// Запустить процесс обработки новых данных
        /// </summary>
        /// <returns></returns>
        public IDictionary<ITransactionsPeriod, bool> RunProcess();

        /// <summary>
        /// Запустить обработку одного пакета данных
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public bool Build(ITransactionsPeriod period);
    }
}
