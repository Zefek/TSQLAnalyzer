using Microsoft.SqlServer.Management.SqlParser.Parser;
using Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;
using SqlAnalyzer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlAnalyzer.Analyzers
{
    public class VariableCountFetchCursorDeclare : SqlCodeObjectRecursiveVisitor, IAnalyzer
    {
        internal const string Message = "The number of variables in a FETCH statement should match the number of columns in the cursor";

        private readonly Dictionary<string, int> cursorVariableCount = new Dictionary<string, int>();
        private readonly List<SqlCodeObject> fetchStatements = new List<SqlCodeObject>();

        private string currentCursor = string.Empty;
        public string Name => "Fetch variable count is same as cursor declare";

        public IEnumerable<DiagnosticMessage> Analyze(SqlScript script)
        {
            cursorVariableCount.Clear();
            fetchStatements.Clear();
            Visit(script);
            return fetchStatements.Select(k => DiagnosticMessage.Warning(new Span(k.StartLocation.Offset, k.Length), Message));
        }

        public override void Visit(SqlCursorDeclareStatement codeObject)
        {
            currentCursor = codeObject.Name.Value.ToLower();
            if (!cursorVariableCount.ContainsKey(currentCursor))
            {
                cursorVariableCount.Add(currentCursor, 0);
                base.Visit(codeObject);
            }
            currentCursor = string.Empty;
        }
        public override void Visit(SqlSelectClause codeObject)
        {
            if (!string.IsNullOrEmpty(currentCursor))
                cursorVariableCount[currentCursor] = codeObject.SelectExpressions.Count;
        }
        public override void Visit(SqlStatement codeObject)
        {
            if (codeObject.Statement is SqlNullStatement statement && statement.Statement.Sql.ToLower().StartsWith("fetch"))
            {
                var cursorName = codeObject.Tokens.LastOrDefault(k => k.Id == (int)Tokens.TOKEN_ID)?.Text;
                if (!string.IsNullOrEmpty(cursorName) && cursorVariableCount.TryGetValue(cursorName.ToLower(), out int variables))
                {
                    if (codeObject.Tokens.Count(k => k.Id == (int)Tokens.TOKEN_VARIABLE) != variables)
                        fetchStatements.Add(codeObject);
                }
            }
        }
    }
}
