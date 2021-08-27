using SqlAnalyzer.Analyzers;
using SqlAnalyzer.DTO;
using System.Collections.Generic;

namespace SqlAnalyzer
{
    public interface ISqlAnalyzerService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="analyzer"></param>
        /// <exception cref="InvalidOperationException">When same analyzer by name added.</exception>
        void AddAnalyzer(IAnalyzer analyzer);
        IEnumerable<DiagnosticMessage> Analyze(string text);
    }
}