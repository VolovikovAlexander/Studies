using LegacyCore.Interfaces;
using LegacyCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LegacyCore.Logic
{
    /// <summary>
    /// Реализация интерфейса <see cref="ILegacyManager"/> для задач по подготовки данных.
    /// </summary>
    public class LegacyManager : ILegacyManager
    {
        private ITransactions _transactions;
        public ITransactions transactions { get => _transactions; set => _transactions = value; }

        /// <summary>
        /// Подготовка данных в разрезе одного периода
        /// </summary>
        /// <param name="period"></param>
        /// <returns></returns>
        public bool Build(ITransactionsPeriod period)
        {
            if (period is null)
                throw new ArgumentNullException("Некорректно переданы параметры!", nameof(period));

            if(period .StartPeriod >= period .StopPeriod)
                throw new ArgumentException("Некорректно переданы параметры!", nameof(period));

            if (!this.IsSuccess(period)) return false;
            var dbPeriod = GetPeriod(period);

            using (var context = new legacyContext())
            {
                // Удалим транзации за указанный период из таблицы tblTransactions
                var containtsItems = context.TblTransactions.Where(x => x.Period == dbPeriod);
                context.TblTransactions.RemoveRange(containtsItems.ToArray());

                // Получим набор исходных данных из таблмцы tblTransactionFacts
                var items = context.TblTransactionFacts.Where(x => x.Period > period.StartPeriod
                   && x.Period <= period.StopPeriod);

                #region Подготовка справочников

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

                // Сохраним данные
                context.SaveChanges();

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

                // Сохраним данные
                context.SaveChanges();

                #endregion

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
            }

            return true;
        }

        public IDictionary<ITransactionsPeriod, bool> RunProcess()
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Проверить наличие записей за указанный период
        /// </summary>
        /// <param name="period"></param>
        /// <returns></returns>
        private bool IsSuccess(ITransactionsPeriod period)
        {
            using (var context = new legacyContext())
            {
                var check = context.TblTransactionFacts.Any(x => x.Period > period.StartPeriod
                                && x.Period <= period.StopPeriod);
                return check;
            }
        }

        /// <summary>
        /// Получить объект типа <see cref="RefReportPeriod"/>
        /// </summary>
        /// <param name="period"></param>
        /// <returns></returns>
        private RefReportPeriod GetPeriod(ITransactionsPeriod period)
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
