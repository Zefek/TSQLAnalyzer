using Microsoft.SqlServer.Management.SqlParser.Parser;
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
    public class UnusedVariableAnalyzer : SqlCodeObjectRecursiveVisitor, IAnalyzer
    {
        internal const string Message = "Variable {0} is not used.";
        private readonly List<SqlVariableDeclaration> declared = new List<SqlVariableDeclaration>();
        private readonly List<string> used = new List<string>();

        public string Name => "Unused variables analyzer";
        public IEnumerable<DiagnosticMessage> Analyze(SqlScript script)
        {
            declared.Clear();
            used.Clear();
            Visit(script);
            return declared
                .Where(k => !used.Contains(k.Name.ToLower()))
                .Select(l => DiagnosticMessage.Warning(new Span(l.StartLocation.Offset, l.Length), string.Format(Message, l.Name)));
        }

        public override void Visit(SqlVariableDeclaration codeObject)
        {
            declared.Add(codeObject);
            base.Visit(codeObject);
        }

        public override void Visit(SqlScalarVariableRefExpression codeObject)
        {
            if (!(codeObject.Parent is SqlVariableAssignment))
                used.Add(codeObject.VariableName.ToLower());
            base.Visit(codeObject);
        }

        public override void Visit(SqlParameterDeclaration codeObject)
        {
            declared.Add(codeObject);
            base.Visit(codeObject);
        }

        public override void Visit(SqlExecuteArgument codeObject)
        {
            if (codeObject.Value is SqlScalarVariableRefExpression variable)
                used.Add(variable.VariableName.ToLower());
        }
        public override void Visit(SqlScalarVariableAssignment codeObject)
        {
            if (codeObject.Value is SqlScalarVariableRefExpression variable)
                used.Add(variable.VariableName.ToLower());
            base.Visit(codeObject);
        }

        public override void Visit(SqlInsertStatement codeObject)
        {
            if (codeObject.InsertSpecification.Source is SqlNullInsertSource)
            {
                var result = Parser.Parse(codeObject.InsertSpecification.Source.Sql);
                Visit(result.Script);
            }
            base.Visit(codeObject);
        }

        public override void Visit(SqlStatement codeObject)
        {
            if (codeObject is SqlNullStatement statement && statement.Sql.ToLower().StartsWith("print"))
                used.AddRange(statement.Tokens.Where(k => k.Id == (int)Tokens.TOKEN_VARIABLE).Select(k => k.Text.ToLower()).Distinct());
        }
    }
}
