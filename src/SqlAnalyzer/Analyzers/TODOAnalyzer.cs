using Microsoft.SqlServer.Management.SqlParser.Parser;
using Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;
using SqlAnalyzer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace SqlAnalyzer.Analyzers
{
    public class TODOAnalyzer : IAnalyzer
    {
        internal const string Message = "TODO Comment should be done";
        private readonly Regex todoRegex = new Regex("todo");
        public string Name => "TODO Analyzer";

        public IEnumerable<DiagnosticMessage> Analyze(SqlScript script)
        {
            foreach(var comment in script.Tokens.Where(k=>k.Id == (int)Tokens.LEX_END_OF_LINE_COMMENT || k.Id==(int)Tokens.LEX_MULTILINE_COMMENT))
            {
                var match = todoRegex.Match(comment.Text.ToLower());
                if(match.Success)
                {
                    yield return DiagnosticMessage.Warning(new Span(comment.StartLocation.Offset + match.Index, match.Length), Message); 
                }
            }
        }
    }
}
