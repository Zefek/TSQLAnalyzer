﻿using Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;
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
        private const string Message = "TOP without ORDER BY";

        private readonly List<SqlTopSpecification> topWithoutOrder = new List<SqlTopSpecification>();

        public string Name => "Top without order by analyzer";
        public IEnumerable<DiagnosticMessage> Analyze(SqlScript script)
        {
            topWithoutOrder.Clear();
            Visit(script);
            foreach (var top in topWithoutOrder)
            {
                yield return new DiagnosticMessage(new Span(top.StartLocation.Offset, top.Length), Message, Severity.Warning);
            }
        }

        public override void Visit(SqlQuerySpecification codeObject)
        {
            if (codeObject.Children.OfType<SqlSelectClause>().Any(k => k.Top != null) && codeObject.Parent is SqlSelectSpecification parent && parent.OrderByClause == null)
                topWithoutOrder.Add(codeObject.Children.OfType<SqlSelectClause>().First().Top);
            base.Visit(codeObject);
        }
    }
}
