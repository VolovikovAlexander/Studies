using LegacyCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LegacyCore.Logic
{
    /// <summary>
    /// Повторная реализация интерфейса <see cref="IBuildManager"/>
    /// </summary>
    public class BuildManagerThreads: BuildManager
    {
        public override bool Build(ITransactionsPeriod period)
        {
            if (period is null)
                throw new ArgumentNullException("Некорректно переданы параметры!", nameof(period));

            if (period.StartPeriod >= period.StopPeriod)
                throw new ArgumentException("Некорректно переданы параметры!", nameof(period));

            if (!this.IsSuccess(period)) return false;

            _logic = _logic ?? new LogicBuildTransactions();

            // 1 Удалить старые записи
            var removeRecord = Task.Factory.StartNew(() => _logic.RemoveRecords(period));

            // 2. Добавим новых клиентов
            var addCustomers = Task.Factory.StartNew(() => _logic.AddCustomers(period));

            // 3 Добавим счета
            var addAccounts = Task.Factory.StartNew(() => _logic.AddAccounts(period));

            // 4 Добавим контракты
            var addContracts = Task.Factory.StartNew(() => _logic.AddContracts(period));

            // 5 Добавим транзации
            var addTransactions = Task.Factory.StartNew(() => _logic.AddTransactions(period));

            var firtsGroup = new List<Task>()
            { removeRecord, addCustomers, addAccounts, addContracts };

            var secondGroup = new List<Task>()
            { addTransactions };


            // Запускаем задачи
            Task.WaitAll(firtsGroup.ToArray());
            Task.WaitAll(secondGroup.ToArray());

            return true;
        }
    }
}
