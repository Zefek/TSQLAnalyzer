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
    public class NullComparisonTests
    {
        [TestMethod()]
        public void AnalyzeNonComplaintTest()
        {
            string sql = @"UPDATE table
                            SET Column = 'Test'
                            WHERE Column2 = NULL -- Noncompliant";
            var result = new NullComparison().Analyze(SqlParser.Parse(sql));
            Assert.IsTrue(result.Any() && result.All(k => k.Message == NullComparison.Message));
        }

        [TestMethod()]
        public void AnalyzeComplaintTest()
        {
            string sql = @"UPDATE table
                            SET Column = 'Test'
                            WHERE Column2 IS NULL";
            var result = new NullComparison().Analyze(SqlParser.Parse(sql));
            Assert.IsFalse(result.Any());
        }
    }
}