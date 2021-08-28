using LegacyCore.Interfaces;
using LegacyCore.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace LegacyTest
{
    [TestClass]
    public class TransactionsTests
    {
        /// <summary>
        /// Проверить метод IsSuccess
        /// </summary>
        [TestMethod]
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

        /// <summary>
        /// Проверить расчет периодов
        /// </summary>
        [TestMethod]
        public void Check_Transaction_CalsPeriods()
        {
            // Подготовка
            var startPeriod = new DateTime(2021, 01, 10);
            var stopPeriod = new DateTime(2021, 03, 12);

            // Результат
            var result = TransactionsPeriod.CalcPeriods(startPeriod, stopPeriod);

            // Проверка
            Assert.AreEqual(4, result.Count());
            Assert.AreEqual(startPeriod, result.First());
            Assert.AreEqual(stopPeriod, result.Last());
        }

        /// <summary>
        /// Проверить расчет периодов при граничном условии
        /// </summary>
        [TestMethod]
        public void Check_Transaction_CalcPeriods_OneMonth()
        {
            // Подготовка
            var startPeriod = new DateTime(2021, 02, 10);
            var stopPeriod = new DateTime(2021, 02, 28);

            // Результат
            var result = TransactionsPeriod.CalcPeriods(startPeriod, stopPeriod);

            // Проверка
            Assert.AreEqual(2, result.Count());
            Assert.AreEqual(startPeriod, result.First());
            Assert.AreEqual(stopPeriod, result.Last());
        }

        /// <summary>
        /// Проверить получение периодов
        /// </summary>
        [TestMethod]
        public void Check_Transaction_GetPeriods()
        {
            // Подготовка
            var transactions = new Transactions();

            // Действие
            var result = transactions.GetPeriods();

            // Проверки
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result.Count() > 0);
            Console.WriteLine($"Записей {result.Count()}");
        }
    }
}
