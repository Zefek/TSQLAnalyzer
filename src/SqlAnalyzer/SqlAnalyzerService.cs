using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Management.SqlParser.Parser;
using SqlAnalyzer.Analyzers;
using SqlAnalyzer.DTO;

namespace SqlAnalyzer
{
    public class SqlAnalyzerService : ISqlAnalyzerService
    {
        private readonly List<IAnalyzer> analyzers = new List<IAnalyzer>();

        public SqlAnalyzerService()
        {
            AddAnalyzer(new SchemaAnalyzer());
            AddAnalyzer(new UnusedVariableAnalyzer());
            AddAnalyzer(new TopWithoutOrderAnalyzer());
            AddAnalyzer(new CommentAnalyzer());
        }

        public void AddAnalyzer(IAnalyzer analyzer)
        {
            if (analyzers.Any(k => k.Name.ToLower() == analyzer.Name.ToLower()))
                throw new InvalidOperationException("Analyzer with same name exists.");
            analyzers.Add(analyzer);
        }
        public IEnumerable<DiagnosticMessage> Analyze(string text)
        {
            var result = Parser.Parse(text);
            List<DiagnosticMessage> messages = new List<DiagnosticMessage>();
            foreach (var analyzer in analyzers)
                messages.AddRange(analyzer.Analyze(result.Script));
            return messages;
        }
    }
}
