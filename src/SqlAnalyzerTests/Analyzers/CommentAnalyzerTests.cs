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
    public class CommentAnalyzerTests
    {
        [TestMethod()]
        public void AnalyzeTest()
        {
            string sql = @"--Test:";
            var result = new CommentAnalyzer().Analyze(SqlParser.Parse(sql));
            Assert.IsFalse(result.Any());
        }
    }
}