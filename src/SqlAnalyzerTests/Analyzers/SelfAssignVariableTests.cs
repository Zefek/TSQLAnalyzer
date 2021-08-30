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
    public class SelfAssignVariableTests
    {
        [TestMethod()]
        public void AnalyzeTest()
        {
            string sql = @"DECLARE @a int = 5
                           SET @a = @a";
            var result = new SelfAssignVariable().Analyze(SqlParser.Parse(sql));
            Assert.IsTrue(result.Any() && result.All(k => k.Message == SelfAssignVariable.Message));
        }

        [TestMethod()]
        public void AnalyzeAddTest()
        {
            string sql = @"DECLARE @a int = 5
                           SET @a = @a + 1";
            var result = new SelfAssignVariable().Analyze(SqlParser.Parse(sql));
            Assert.IsFalse(result.Any());
        }

        [TestMethod()]
        public void AnalyzeSelectTest()
        {
            string sql = @"DECLARE @a int = 5
                           SELECT @a = @a";
            var result = new SelfAssignVariable().Analyze(SqlParser.Parse(sql));
            Assert.IsTrue(result.Any() && result.All(k => k.Message == SelfAssignVariable.Message));
        }
    }
}