using Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;
using SqlAnalyzer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlAnalyzer.Analyzers
{
    public class SelfAssignVariable : SqlCodeObjectRecursiveVisitor, IAnalyzer
    {
        internal const string Message = "Variable assigned by self should not be";
        private readonly List<SqlCodeObject> objects = new List<SqlCodeObject>();
        public string Name => "Self assign variable analyzer";

        public IEnumerable<DiagnosticMessage> Analyze(SqlScript script)
        {
            objects.Clear();
            Visit(script);
            return objects.Select(k => DiagnosticMessage.Warning(new Span(k.StartLocation.Offset, k.Length), Message));
        }

        public override void Visit(SqlScalarVariableAssignment codeObject)
        {
            if (codeObject.Value is SqlScalarVariableRefExpression variable && codeObject.Variable.VariableName == variable.VariableName)
                objects.Add(codeObject);
        }
    }
}
