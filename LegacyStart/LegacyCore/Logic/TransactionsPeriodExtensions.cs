using LegacyCore.Interfaces;
using LegacyCore.Models;

namespace LegacyCore.Logic
{
    public static class TransactionsPeriodExtensions
    {
        /// <summary>
        /// Расширение на интерфейс <see cref="ITransactionsPeriod"/>. Конвертирует в модель <see cref="RefReportPeriod"/>
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static RefReportPeriod ToRefReportPeriod(this ITransactionsPeriod source)
        {
            if (source is null)
                throw new System.ArgumentNullException("Некорректно переданы параметры!", nameof(source));

            return new RefReportPeriod()
            {
                StartPeriod = source.StartPeriod,
                StopPeriod = source.StopPeriod,
                Comments = $"{source.StartPeriod.ToString("dd.MM.yyyy")}-{source.StopPeriod.ToString("dd.MM.yyyy")}"
            };
        }
    }
}
