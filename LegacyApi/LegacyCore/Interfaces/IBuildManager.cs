using System.Collections.Generic;

namespace LegacyCore.Interfaces
{
    // <summary>
    /// Интерфейс к бизнес системе обработки новых данных
    /// </summary>
    public interface IBuildManager
    {
        /// <summary>
        /// Объект для работы с новыми данными
        /// </summary>
        public ITransactions transactions { get; set; }

        /// <summary>
        /// Объект для обработки транзакций
        /// </summary>
        public IlogicBuildTransactions logic { get; set; }


        /// <summary>
        /// Запустить обработку одного пакета данных
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public bool Build(ITransactionsPeriod period);
    }
}
