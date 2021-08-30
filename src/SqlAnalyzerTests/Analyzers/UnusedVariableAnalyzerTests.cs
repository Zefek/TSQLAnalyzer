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
    public class UnusedVariableAnalyzerTests
    {
        [TestMethod()]
        public void AnalyzeTest()
        {
            string sql = @"DECLARE @Test BIT = 1
                           SET @Test = 1";
            var result = new UnusedVariableAnalyzer().Analyze(SqlParser.Parse(sql));
            Assert.IsTrue(result.Any() && result.All(k => k.Message == string.Format(UnusedVariableAnalyzer.Message, "@Test")));
        }

        [TestMethod()]
        public void AnalyzeUsedInConditionTest()
        {
            string sql = @"DECLARE @Test BIT = 1
                           IF @Test = 1
                             print 'OK'";
            var result = new UnusedVariableAnalyzer().Analyze(SqlParser.Parse(sql));
            Assert.IsFalse(result.Any());
        }

        [TestMethod()]
        public void AnalyzeUsedInProcedureTest()
        {
            string sql = @"DECLARE @Test BIT = 1
                           EXEC dbo.Test @A = @Test";
            var result = new UnusedVariableAnalyzer().Analyze(SqlParser.Parse(sql));
            Assert.IsFalse(result.Any());
        }

        [TestMethod()]
        public void AnalyzeUsedInInsertIntoFromProcedureTest()
        {
            string sql = @"DECLARE @Test BIT = 1
                           INSERT INTO @Table
                           EXEC dbo.Test @A = @Test";
            var result = new UnusedVariableAnalyzer().Analyze(SqlParser.Parse(sql));
            Assert.IsFalse(result.Any());
        }

        [TestMethod()]
        public void AnalyzeUsedInInsertTest()
        {
            string sql = @"DECLARE @Test BIT = 1
                           INSERT INTO Table (Test) values (@Test)";
            var result = new UnusedVariableAnalyzer().Analyze(SqlParser.Parse(sql));
            Assert.IsFalse(result.Any());
        }
    }
}