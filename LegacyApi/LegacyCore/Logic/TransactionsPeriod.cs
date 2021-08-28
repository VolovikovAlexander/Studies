using LegacyCore.Interfaces;
using System;
using System.Collections.Generic;

namespace LegacyCore.Logic
{
    public class TransactionsPeriod : ITransactionsPeriod
    {
        private DateTime _startPeriod = new DateTime(1900, 1, 1);
        private DateTime _stopPeriod = DateTime.Now;
        public DateTime StartPeriod { get => _startPeriod; set => _startPeriod = value; }
        public DateTime StopPeriod { get => _stopPeriod; set => _stopPeriod = value; }

        /// <summary>
        /// По указанным датам вернуть список дат - начало месяца
        /// </summary>
        /// <param name="startPeriod"></param>
        /// <param name="stopPeriod"></param>
        /// <returns></returns>
        public static IEnumerable<DateTime> CalcPeriods(DateTime startPeriod, DateTime stopPeriod)
        {
            if (startPeriod >= stopPeriod)
                throw new ArgumentException($"Некорректно переданы параетры! Начальны период {startPeriod} >= {stopPeriod}");

            var result = new List<DateTime>();
            result.Add(startPeriod);

            var currentPeriod = new DateTime(startPeriod.Year, startPeriod.Month, 1)
                            .AddMonths(1);
            currentPeriod = new DateTime(currentPeriod.Year, currentPeriod.Month, 1);

            while (currentPeriod <= stopPeriod)
            {
                result.Add(currentPeriod);
                currentPeriod = currentPeriod.AddMonths(1);
            }

            result.Add(stopPeriod);
            return result;
        }
    }
}
