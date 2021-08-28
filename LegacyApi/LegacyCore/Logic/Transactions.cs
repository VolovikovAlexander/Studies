using LegacyCore.Interfaces;
using LegacyCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LegacyCore.Logic
{
    /// <summary>
    /// Реализация интерфейса для работы с исходными данными <see cref="ITransactions"/>
    /// </summary>
    public class Transactions : ITransactions
    {

        #region Реализация ITransactions

        /// <summary>
        /// Проверить наличие новых записей
        /// </summary>
        public bool IsSuccess
        {
            get
            {
                // Последняя запись в таблице периодов
                var lastPeriod = this.LastPeriod;

                using (var context = new legacyContext())
                {
                    // Есть ли записи в таблице фактов соответствующие критериям
                    var success = context.TblTransactionFacts
                            .Any(x => x.Period > lastPeriod);
                    return success;
                }
            }
        }

        /// <summary>
        /// Получить список записей который подпадает под указанные критерии
        /// </summary>
        /// <param name="period"></param>
        /// <returns></returns>
        public IEnumerable<IModel> GetNextRecords(ITransactionsPeriod period)
        {
            if (period is null)
                throw new ArgumentNullException("Некорректно переданы паораметры!", nameof(period));

            IEnumerable<IModel> result = Enumerable.Empty<IModel>();

            using (var context = new legacyContext())
            {
                var items = context.TblTransactionFacts
                    .Where(x => x.Period > period.StartPeriod && x.Period <= period.StopPeriod)
                    .Select(x => x as IModel);

                result = items.ToList();
            }

            return result;
        }

        /// <summary>
        /// Получить список периодов с разбивкой по месяцам
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ITransactionsPeriod> GetPeriods()
        {
            var lastPeriod = this.LastPeriod;
            var currentperiod = this.CurrentPeriod;


            throw new NotImplementedException();
        }

        #endregion

        /// <summary>
        /// По указанным датам вернуть список дат - начало месяца
        /// </summary>
        /// <param name="startPeriod"></param>
        /// <param name="stopPeriod"></param>
        /// <returns></returns>
        public IEnumerable<DateTime> CalcPeriods(DateTime startPeriod, DateTime stopPeriod)
        {
            if (startPeriod >= stopPeriod)
                throw new ArgumentException($"Некорректно переданы параетры! Начальны период {startPeriod} >= {stopPeriod}");

            return null;
        }


        /// <summary>
        /// Последняя дата в справочнике refReportPeriods
        /// </summary>
        private DateTime LastPeriod
        {
            get
            {
                using (var context = new legacyContext())
                {
                    // Последняя запись в таблице периодов
                    var lastPeriod = context.RefReportPeriods.OrderByDescending(x => x.Period)
                            .LastOrDefault()?.Period ?? new DateTime(1900, 1, 1);

                    return lastPeriod;
                }
            }
        }

        /// <summary>
        /// Получить последнюю дату в таблице фактов tblTransactionFacts
        /// </summary>
        private DateTime CurrentPeriod
        {
            get
            {
                using (var context = new legacyContext())
                {
                    // Последняя запись в таблице периодов
                    var lastPeriod = context.TblTransactionFacts.OrderByDescending(x => x.Period)
                            .LastOrDefault()?.Period ?? DateTime.Now;

                    return lastPeriod;
                }
            }
        }

    }
}
