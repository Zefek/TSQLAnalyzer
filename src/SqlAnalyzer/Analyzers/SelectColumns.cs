using Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;
using SqlAnalyzer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlAnalyzer.Analyzers
{
    public class SelectColumns : SqlCodeObjectRecursiveVisitor, IAnalyzer
    {
        internal const string Message= "Select should have columns defined";
        private readonly List<SqlCodeObject> objects = new List<SqlCodeObject>();
        public string Name => "Select columns analyzer";

        public IEnumerable<DiagnosticMessage> Analyze(SqlScript script)
        {
            objects.Clear();
            Visit(script);
            return objects.Select(k => DiagnosticMessage.Warning(new Span(k.StartLocation.Offset, k.Length), Message));
        }

        public override void Visit(SqlSelectClause codeObject)
        {
            if (codeObject.SelectExpressions.OfType<SqlSelectStarExpression>().Any())
                objects.Add(codeObject);
        }
    }
}
