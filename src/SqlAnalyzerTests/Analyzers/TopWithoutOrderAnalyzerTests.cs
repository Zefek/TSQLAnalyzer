using Microsoft.VisualStudio.TestTools.UnitTesting;
using SqlAnalyzer.Analyzers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlAnalyzer.Analyzers.Tests
{
    [TestClass()]
    public class TopWithoutOrderAnalyzerTests
    {
        [TestMethod()]
        public void AnalyzeNonComplaintTest()
        {
            string sql = @"SELECT TOP 10
                          Column1, Column2
                          FROM table
                          WHERE Column3 IS NOT NULL;

                          DELETE TOP (10)
                          FROM Table
                          WHERE Column1 < @Column2Value;";
            var result = new TopWithoutOrderAnalyzer().Analyze(SqlParser.Parse(sql));
            Assert.IsTrue(result.Any() && result.All(k => k.Message == TopWithoutOrderAnalyzer.Message));
        }

        [TestMethod()]
        public void AnalyzeComplaintTest()
        {
            string sql = @"SELECT TOP 10
                          Column1, Column2
                          FROM table
                          WHERE Column1 IS NOT NULL
                          ORDER BY Column2;

                        DELETE
                          FROM Table
                          WHERE Column1 IN (
                            SELECT TOP 10
                              Column1
                              FROM Table2
                              WHERE Column2 < @Column2Value
                              ORDER BY Column3 ASC
                          );";
            var result = new TopWithoutOrderAnalyzer().Analyze(SqlParser.Parse(sql));
            Assert.IsFalse(result.Any());
        }
    }
}