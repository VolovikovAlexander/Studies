using LegacyCore.Interfaces;
using LegacyCore.Models;
using System.Collections.Generic;
using System.Linq;

namespace LegacyCore.Logic
{
    /// <summary>
    /// Реализация интерфейса <see cref="IlogicBuildTransactions"/>
    /// </summary>
    public class LogicBuildTransactions : IlogicBuildTransactions
    {
        #region Реализация IlogicBuildTransactions

        /// <summary>
        /// Добавить новые счета
        /// </summary>
        /// <param name="source"></param>
        /// <param name="period"></param>
        /// <returns></returns>
        public virtual bool AddAccounts(ITransactionsPeriod period)
        {
            using var context = new legacyContext();
            var items = this.GetTransactions(context, period);

            // Получим уникальный список счетов
            var accounts = from s in items
                            group s by new { s.AccountNumber, s.Category } into g
                            select new { AccountNumber = g.Key.AccountNumber, AccountType = g.Key.Category };

            // Справочник лимитов
            var limits = new Dictionary<string, decimal>()
            {
                {"AC", 500000 }, {"BB", 800}, {"FA", 76760}
            };

            // Определим новые счета
            var freshAccounts = accounts
                        .Where(x => !context.RefAccounts.Any(y => y.AccountNumber == x.AccountNumber))
                        .Select(x => new RefAccount()
                        {
                            AccountNumber = x.AccountNumber,
                            AccountType = x.AccountType,
                            LimitAmount = limits.ContainsKey(x.AccountType) ? limits[x.AccountType] : 0
                        });
            context.RefAccounts.AddRange(freshAccounts.ToArray());

            // Сохраним данные
            context.SaveChanges();

            return true;
        }

        /// <summary>
        /// Добавить новые контракты
        /// </summary>
        /// <param name="source"></param>
        /// <param name="period"></param>
        /// <returns></returns>
        public virtual bool AddContracts( ITransactionsPeriod period)
        {
            using var context = new legacyContext();
            var items = this.GetTransactions(context, period);

            // Получаем новые контракты
            var contracts = from s in items
                            group s by new { s.ContractNumber } into g
                            select new { ContractNumber = g.Key.ContractNumber };

            var freshContracts = contracts
                        .Where(x => !context.RefContracts.Any(y => y.ContractNumber == x.ContractNumber))
                        .Select(x => new RefContract()
                        {
                            ContractNumber = x.ContractNumber
                        });

            context.RefContracts.AddRange(freshContracts.ToArray());

        return true;
        }

        /// <summary>
        /// Добавить новых клиентов
        /// </summary>
        /// <param name="source"></param>
        /// <param name="period"></param>
        /// <returns></returns>
        public virtual bool AddCustomers(ITransactionsPeriod period)
        {
            using var context = new legacyContext();
            var items = this.GetTransactions(context, period);

            // Получим уникальный список клиентов
            var customers = from s in items
                            group s by new { s.CustomerNumber, s.CustomerInn } into g
                            select new { CustomerName = g.Key.CustomerNumber, Inn = g.Key.CustomerInn };

            // Определим новые
            var freshCustomers = customers
                        .Where(x => !context.RefCustomers.Any(y => y.Description == x.CustomerName))
                        .Select(x => new RefCustomer()
                        {
                        // Наименование
                        Description = x.CustomerName,
                        // ИНН
                        Comments = x.Inn,
                        // Тип клиента
                        CustomerType = x.CustomerName.StartsWith("З") ? "Нерезидент" : ""
                        });

            context.RefCustomers.AddRange(freshCustomers.ToArray());
            return true;
        }

        /// <summary>
        /// Добавить обработанные транзакции
        /// </summary>
        /// <param name="period"></param>
        /// <returns></returns>
        public virtual bool AddTransactions( ITransactionsPeriod period)
        {
            using var context = new legacyContext();
            var items = this.GetTransactions(context, period);

            var dbPeriod = GetPeriod(period);

            // Переносим транзакции в подготовленную таблицу 
            var transactions = items
            .Select(x => new TblTransaction()
            {
                Account = context.RefAccounts.Where(y => y.AccountNumber == x.AccountNumber).First(),
                Amount = x.Amount,
                Contract = context.RefContracts.Where(y => y.ContractNumber == x.ContractNumber).First(),
                Customer = context.RefCustomers.Where(y => y.Description == x.CustomerNumber).First(),
                PeriodId = dbPeriod.Id,
                OperationType = x.Amount >= 0 ? "Кредит" : "Дебет",
                TransactionNumber = x.TransactionNumber,
                TransactionPeriod = x.Period
            });

            context.TblTransactions.AddRange(transactions.ToArray());

            // Сохраним данные
            context.SaveChanges();
            return true;
        }

        /// <summary>
        /// Получить выборку данных по указанному периоду
        /// </summary>
        /// <param name="context"></param>
        /// <param name="period"></param>
        /// <returns></returns>
        public virtual IQueryable<TblTransactionFact> GetTransactions(legacyContext context, ITransactionsPeriod period)
        {
             // Получим набор исходных данных из таблмцы tblTransactionFacts
             var items = context.TblTransactionFacts.Where(x => x.Period > period.StartPeriod
                   && x.Period <= period.StopPeriod);

             return items;
        }

        /// <summary>
        /// Удалить записи из подготовленной таблицы
        /// </summary>
        /// <param name="period"></param>
        /// <returns></returns>
        public virtual bool RemoveRecords(ITransactionsPeriod period)
        {
            var dbPeriod = GetPeriod(period);

            using var context = new legacyContext();
            // Удалим транзации за указанный период из таблицы tblTransactions
            var containtsItems = context.TblTransactions.Where(x => x.Period == dbPeriod);
            context.TblTransactions.RemoveRange(containtsItems.ToArray());

            context.SaveChanges();
            return true;
        }

        #endregion


        /// <summary>
        /// Получить объект типа <see cref="RefReportPeriod"/>
        /// </summary>
        /// <param name="period"></param>
        /// <returns></returns>
        public static RefReportPeriod GetPeriod(ITransactionsPeriod period)
        {
            using (var context = new legacyContext())
            {
                var item = context.RefReportPeriods.Where(x => x.StartPeriod == period.StartPeriod &&
                           x.StopPeriod == period.StopPeriod).FirstOrDefault();

                if (item is null)
                {
                    // Добавим период
                    var result = period.ToRefReportPeriod();
                    context.RefReportPeriods.Add(result);

                    context.SaveChanges();
                    return GetPeriod(period);
                }
                else
                    return item;
            }
        }
    }
}
