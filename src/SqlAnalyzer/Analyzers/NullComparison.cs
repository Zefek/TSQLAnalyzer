using Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;
using SqlAnalyzer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlAnalyzer.Analyzers
{
    public class NullComparison : SqlCodeObjectRecursiveVisitor, IAnalyzer
    {
        internal const string Message = "NULL should be compared by IS NULL";
        private readonly List<SqlCodeObject> nullComparison = new List<SqlCodeObject>();
        public string Name => "Null comparison analyzer";

        public IEnumerable<DiagnosticMessage> Analyze(SqlScript script)
        {
            nullComparison.Clear();
            Visit(script);
            return nullComparison.Select(k => DiagnosticMessage.Warning(new Span(k.StartLocation.Offset, k.Length), Message));
        }


        public override void Visit(SqlComparisonBooleanExpression codeObject)
        {
            if ((codeObject.Left is SqlLiteralExpression leftExpression && leftExpression.Type == LiteralValueType.Null) || (codeObject.Right is SqlLiteralExpression rightExpression && rightExpression.Type == LiteralValueType.Null))
                nullComparison.Add(codeObject);
        }
    }
}
