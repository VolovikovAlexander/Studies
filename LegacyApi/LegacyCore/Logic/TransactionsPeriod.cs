using LegacyCore.Interfaces;
using System;

namespace LegacyCore.Logic
{
    public class TransactionsPeriod : ITransactionsPeriod
    {
        private DateTime _startPeriod = new DateTime(1900, 1, 1);
        private DateTime _stopPeriod = DateTime.Now;
        public DateTime StartPeriod { get => _startPeriod; set => _startPeriod = value; }
        public DateTime StopPeriod { get => _stopPeriod; set => _stopPeriod = value; }
    }
}
