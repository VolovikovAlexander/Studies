using LegacyCore.Interfaces;
using LegacyCore.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;


namespace LegacyTest
{
    [TestClass]
    public class Logic
    {
        /// <summary>
        /// Проверить обработку транзакций
        /// </summary>
        [TestMethod]
        public void Check_AddTransactions()
        {
            // Подготовка
            var logic = new logicBuildTransactions();
            var period = new TransactionsPeriod()
            {
                StartPeriod = new DateTime(2021, 1, 1),
                StopPeriod = new DateTime(2021, 2, 1)
            };

            // Действие
            var result = logic.AddTransactions(period);

            // Проверка
            Assert.AreEqual(true, result);
        }

        /// <summary>
        /// Проверить работу удаления записей
        /// </summary>
        [TestMethod]
        public void Check_RemoveRecords()
        {
            // Подготовка
            var logic = new logicBuildTransactions();
            var period = new TransactionsPeriod()
            {
                StartPeriod = new DateTime(2021, 1, 1),
                StopPeriod = new DateTime(2021, 2, 1)
            };

            // Действие
            var result = logic.RemoveRecord(period);

            // Проверка
            Assert.AreEqual(true, result);
        }

        /// <summary>
        /// Проверить работу добавления новых счетов
        /// </summary>
        [TestMethod]
        public void Check_AddAccounts()
        {
            // Подготовка
            var logic = new logicBuildTransactions();
            var period = new TransactionsPeriod()
            {
                StartPeriod = new DateTime(2021, 1, 1),
                StopPeriod = new DateTime(2021, 2, 1)
            };

            // Действие
            var result = logic.AddAccounts(period);

            // Проверка
            Assert.AreEqual(true, result);
        }


        /// <summary>
        /// Проверить работу добавления новых клиентов
        /// </summary>
        [TestMethod]
        public void Check_AddCustomers()
        {
            // Подготовка
            var logic = new logicBuildTransactions();
            var period = new TransactionsPeriod()
            {
                StartPeriod = new DateTime(2021, 1, 1),
                StopPeriod = new DateTime(2021, 2, 1)
            };

            // Действие
            var result = logic.AddCustomers(period);

            // Проверка
            Assert.AreEqual(true, result);
        }


        /// <summary>
        /// Проверить работу добавления новых контрактов
        /// </summary>
        [TestMethod]
        public void Check_AddContracts()
        {
            // Подготовка
            var logic = new logicBuildTransactions();
            var period = new TransactionsPeriod()
            {
                StartPeriod = new DateTime(2021, 1, 1),
                StopPeriod = new DateTime(2021, 2, 1)
            };

            // Действие
            var result = logic.AddContracts(period);

            // Проверка
            Assert.AreEqual(true, result);
        }
    }
}
