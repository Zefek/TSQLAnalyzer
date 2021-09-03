using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;
using SqlAnalyzer.DTO;

namespace SqlAnalyzer.Analyzers
{
    internal class SchemaAnalyzer : SqlCodeObjectRecursiveVisitor, IAnalyzer
    {
        internal const string Message = "Object should have schema name (dbo).";

        private readonly List<SqlObjectIdentifier> identifiers = new List<SqlObjectIdentifier>();
        public string Name => "Schema analyzer";
        public IEnumerable<DiagnosticMessage> Analyze(SqlScript script)
        {
            identifiers.Clear();
            Visit(script);
            foreach (var expression in identifiers)
            {
                yield return DiagnosticMessage.Warning(new Span(expression.StartLocation.Offset, expression.Length), Message);
            }
        }

        public override void Visit(SqlTableRefExpression codeObject)
        {
            if (string.IsNullOrEmpty(codeObject.ObjectIdentifier.SchemaName.Value) && !codeObject.ObjectIdentifier.ObjectName.Value.StartsWith("#"))
            {
                if (!(codeObject.Parent is SqlUpdateSpecification) || (codeObject.Parent is SqlUpdateSpecification parent && parent.FromClause == null))
                    identifiers.Add(codeObject.ObjectIdentifier);
            }
            base.Visit(codeObject);
        }

        public override void Visit(SqlExecuteModuleStatement codeObject)
        {
            if (string.IsNullOrEmpty(codeObject.Module.ObjectIdentifier.SchemaName.Value))
                identifiers.Add(codeObject.Module.ObjectIdentifier);
            base.Visit(codeObject);
        }

        public override void Visit(SqlUserDefinedScalarFunctionCallExpression codeObject)
        {
            if (string.IsNullOrEmpty(codeObject.ObjectIdentifier.SchemaName.Value))
                identifiers.Add(codeObject.ObjectIdentifier);
            base.Visit(codeObject);
        }
    }
}
