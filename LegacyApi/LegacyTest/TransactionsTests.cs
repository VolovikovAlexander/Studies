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


    }
}
