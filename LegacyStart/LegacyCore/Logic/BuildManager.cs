using LegacyCore.Interfaces;
using LegacyCore.Models;
using System;
using System.Linq;

namespace LegacyCore.Logic
{
    /// <summary>
    /// Реализация интерфейса <see cref="IBuildManager"/> для задач по подготовки данных.
    /// </summary>
    public class BuildManager : IBuildManager
    {
        protected ITransactions _transactions;
        protected IlogicBuildTransactions _logic;


        #region Свойства
        public ITransactions transactions { get => _transactions; set => _transactions = value; }
        public IlogicBuildTransactions logic { get => _logic; set => _logic = value; }

        #endregion

        #region Реализация

        /// <summary>
        /// Подготовка данных в разрезе одного периода
        /// </summary>
        /// <param name="period"></param>
        /// <returns></returns>
        public virtual bool Build(ITransactionsPeriod period)
        {
            if (period is null)
                throw new ArgumentNullException("Некорректно переданы параметры!", nameof(period));

            if(period .StartPeriod >= period .StopPeriod)
                throw new ArgumentException("Некорректно переданы параметры!", nameof(period));


            if (!this.IsSuccess(period)) return false;

            _logic = _logic ?? new LogicBuildTransactions();

            // 1 Удалить старые записи
            _logic.RemoveRecords(period);

            // 2. Добавим новых клиентов
            _logic.AddCustomers(period);

            // 3 Добавим счета
            _logic.AddAccounts(period);

            // 4 Добавим контракты
            _logic.AddContracts(period);

            // 5 Добавим транзации
            _logic.AddTransactions(period);

            return true;
        }

        #endregion

        /// <summary>
        /// Проверить наличие записей за указанный период
        /// </summary>
        /// <param name="period"></param>
        /// <returns></returns>
        protected bool IsSuccess(ITransactionsPeriod period)
        {
            using (var context = new legacyContext())
            {
                var check = context.TblTransactionFacts.Any(x => x.Period > period.StartPeriod
                                && x.Period <= period.StopPeriod);
                return check;
            }
        }
    }
}
