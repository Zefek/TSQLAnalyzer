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
    public class SelectColumnsTests
    {
        [TestMethod()]
        public void AnalyzeNonComplaintTest()
        {
            string sql = @"SELECT *
                           FROM table
                           WHERE Column = 'Test'";
            var result = new SelectColumns().Analyze(SqlParser.Parse(sql));
            Assert.IsTrue(result.Any() && result.All(k => k.Message == SelectColumns.Message));
        }

        [TestMethod()]
        public void AnalyzeComplaintTest()
        {
            string sql = @"SELECT Column1, Column2
                            FROM table
                            WHERE Column1 = 'Test'";
            var result = new SelectColumns().Analyze(SqlParser.Parse(sql));
            Assert.IsFalse(result.Any());
        }
    }
}