using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;
using SqlAnalyzer.DTO;

namespace SqlAnalyzer.Analyzers
{
    public interface IAnalyzer
    {
        string Name { get; }
        IEnumerable<DiagnosticMessage> Analyze(SqlScript script);
    }
}
