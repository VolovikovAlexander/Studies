using LegacyCore.Interfaces;
using LegacyCore.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace LegacyTest
{
    [TestClass]
    public class Performance
    {
        /// <summary>
        /// Получить максимальную дату tblTransactionFacts
        /// </summary>
        [TestMethod]
        [TestCategory("Performance")]
        public void Check_Transaction_CurrentPeriod()
        {
            // Подготовка
            var transactions = new Transactions();

            // Действие
            var result = transactions.CurrentPeriod;

            // Проверка
            Assert.IsNotNull(result);
            Console.WriteLine($"Максимальная дата в таблице tblTransactionFacts {result}");

        }
        /// <summary>
        /// Проверка работы метода Build
        /// </summary>
        [TestMethod]
        [TestCategory("Performance")]
        public void Check_Manager_Build()
        {
            // Подготовка
            var period = new TransactionsPeriod()
            {
                StartPeriod = new DateTime(2021, 1, 1),
                StopPeriod = new DateTime(2021, 1, 10)
            };
            var manager = new BuildManager();

            // Действие
            var result = manager.Build(period);

            // Проверка
            Assert.AreEqual(true, result);
        }

        /// <summary>
        /// Проверить метод IsSuccess
        /// </summary>
        [TestMethod]
        [TestCategory("Performance")]
        public void Check_Transaction_IsSuccess()
        {
            // Подготовка
            var transactions = new Transactions();

            // Действие
            var result = transactions.IsSuccess;

            // Проверка
            Assert.AreEqual(true, result);
        }

        /// <summary>
        /// Проверить метод GetNextRecords с различными вариантами
        /// </summary>
        /// <param name="source"></param>
        [TestMethod]
        [TestCategory("Performance")]
        public void Check_Transaction_GetNextRecords()
        {
            // Подготовка
            ITransactionsPeriod source = new TransactionsPeriod()
            { StartPeriod = new System.DateTime(2021, 1, 1), StopPeriod = new System.DateTime(2021, 01, 31) };
            var transactions = new Transactions();

            // Действие
            var result = transactions.GetNextRecords(source);

            // Проверки
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result.Any());

            Console.WriteLine($"Записей найдено:{result.Count()}");
        }

    }
}
