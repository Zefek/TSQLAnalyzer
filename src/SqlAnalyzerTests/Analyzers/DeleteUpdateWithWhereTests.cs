using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlAnalyzer.Analyzers.Tests
{
    [TestClass()]
    public class DeleteUpdateWithWhereTests
    {
        [TestMethod()]
        public void AnalyzeNonComplaintTest()
        {
            string sql = @"DELETE FROM table;
                           UPDATE table SET column = 'wait' FROM table1 AS table;";
            var result = new DeleteUpdateWithWhere().Analyze(SqlParser.Parse(sql));
            Assert.IsTrue(result.Any() && result.All(k => k.Message == DeleteUpdateWithWhere.Message));
        }

        [TestMethod()]
        public void AnalyzeComplaintTest()
        {
            string sql = @"TRUNCATE TABLE table;
                            DELETE FROM table WHERE Column = @columnValue;
                            UPDATE table SET Column = 'Test' WHERE Column2 < @column2Value;";
            var result = new DeleteUpdateWithWhere().Analyze(SqlParser.Parse(sql));
            Assert.IsFalse(result.Any());
        }
    }
}