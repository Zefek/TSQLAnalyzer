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
    public class SchemaAnalyzerTests
    {
        [TestMethod()]
        public void AnalyzeTest()
        {
            string sql = @"SELECT Column FROM Table";
            var result = new SchemaAnalyzer().Analyze(SqlParser.Parse(sql));
            Assert.IsTrue(result.Any() && result.All(k => k.Message == SchemaAnalyzer.Message));
        }

        [TestMethod()]
        public void AnalyzeComplaintTest()
        {
            string sql = @"SELECT Column FROM dbo.Table";
            var result = new SchemaAnalyzer().Analyze(SqlParser.Parse(sql));
            Assert.IsFalse(result.Any());
        }

        [TestMethod()]
        public void AnalyzeVariableTableTest()
        {
            string sql = @"SELECT Column FROM @table";
            var result = new SchemaAnalyzer().Analyze(SqlParser.Parse(sql));
            Assert.IsFalse(result.Any());
        }

        [TestMethod()]
        public void AnalyzeTemporaryTableTest()
        {
            string sql = @"SELECT Column FROM #table";
            var result = new SchemaAnalyzer().Analyze(SqlParser.Parse(sql));
            Assert.IsFalse(result.Any());
        }
    }
}