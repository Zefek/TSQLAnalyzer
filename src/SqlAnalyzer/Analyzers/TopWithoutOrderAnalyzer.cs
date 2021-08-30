using Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;
using SqlAnalyzer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SqlAnalyzer.Analyzers
{
    public class TopWithoutOrderAnalyzer : SqlCodeObjectRecursiveVisitor, IAnalyzer
    {
        internal const string Message = "Select with TOP should not be without ORDER BY";

        private readonly List<SqlTopSpecification> topWithoutOrder = new List<SqlTopSpecification>();

        public string Name => "Top without order by analyzer";
        public IEnumerable<DiagnosticMessage> Analyze(SqlScript script)
        {
            topWithoutOrder.Clear();
            Visit(script);
            foreach (var top in topWithoutOrder)
            {
                yield return DiagnosticMessage.Warning(new Span(top.StartLocation.Offset, top.Length), Message);
            }
        }

        public override void Visit(SqlQuerySpecification codeObject)
        {
            if (codeObject.Children.OfType<SqlSelectClause>().Any(k => k.Top != null) && codeObject.Parent is SqlSelectSpecification parent && parent.OrderByClause == null)
                topWithoutOrder.Add(codeObject.Children.OfType<SqlSelectClause>().First().Top);
            base.Visit(codeObject);
        }

        public override void Visit(SqlDeleteSpecification codeObject)
        {
            if (codeObject.TopSpecification != null && !codeObject.Children.OfType<SqlOrderByClause>().Any())
                topWithoutOrder.Add(codeObject.TopSpecification);
            base.Visit(codeObject);
        }
    }
}
