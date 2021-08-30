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
    public class VariableCountFetchCursorDeclareTests
    {
        [TestMethod()]
        public void AnalyzeNonComplaintTest()
        {
            string sql = @"DECLARE cTbl cursor FOR SELECT Column1, Column2 FROM table;
                            OPEN cTbl;
                            FETCH NEXT FROM cTbl INTO @Var;";
            var result = new VariableCountFetchCursorDeclare().Analyze(SqlParser.Parse(sql));
            Assert.IsTrue(result.Any() && result.All(k => k.Message == VariableCountFetchCursorDeclare.Message));
        }

        [TestMethod()]
        public void AnalyzeComplaintTest()
        {
            string sql = @"DECLARE cTbl cursor FOR SELECT Column1, Column2 FROM table;
                        OPEN cTbl;
                        FETCH NEXT FROM cTbl INTO @Var1, @Var2;";
            var result = new VariableCountFetchCursorDeclare().Analyze(SqlParser.Parse(sql));
            Assert.IsFalse(result.Any());
        }

        [TestMethod()]
        public void AnalyzeComplaintTest2()
        {
            string sql = @"DECLARE cTbl CURSOR LOCAL FOR
                            SELECT Column1 FROM Table
    
                            FETCH NEXT FROM cTbl
                            INTO @Var1
                            SELECT Column1, Column2 FROM Table2 Where Column3 = @Var1
                            FETCH NEXT FROM cTbl
                                  INTO @Var1";
            var result = new VariableCountFetchCursorDeclare().Analyze(SqlParser.Parse(sql));
            Assert.IsFalse(result.Any());
        }
    }
}