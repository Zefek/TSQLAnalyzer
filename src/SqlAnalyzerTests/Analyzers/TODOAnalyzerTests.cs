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
    public class TODOAnalyzerTests
    {
        [TestMethod()]
        public void AnalyzeSingleLineTest()
        {
            string sql = "--TODO code to do";
            var result = new TODOAnalyzer().Analyze(SqlParser.Parse(sql));
            Assert.IsTrue(result.Any() && result.All(k => k.Message == TODOAnalyzer.Message));
        }

        [TestMethod()]
        public void AnalyzeMultiLineTest()
        {
            string sql = @"/*Code to do:
                             TODO: Do something*/";
            var result = new TODOAnalyzer().Analyze(SqlParser.Parse(sql));
            Assert.IsTrue(result.Any() && result.All(k => k.Message == TODOAnalyzer.Message));
        }

        [TestMethod()]
        public void AnalyzeNoComentTest()
        {
            string sql = @"--Nothing";
            var result = new TODOAnalyzer().Analyze(SqlParser.Parse(sql));
            Assert.IsFalse(result.Any());
        }
    }
}