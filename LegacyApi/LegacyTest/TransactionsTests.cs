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
        /// Получить максимальную дату в таблице refReportPeriods
        /// </summary>
        [TestMethod]
        public void Check_Transaction_LastPeriod()
        {
            // Подготовка
            var transactions = new Transactions();

            // Действие
            var result = transactions.LastPeriod;

            // Проверка
            Assert.IsNotNull(result);
            Console.WriteLine($"Последний период в истории {result}");
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
        [TestCategory("Performance")]
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
