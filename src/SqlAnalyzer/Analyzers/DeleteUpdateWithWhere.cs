using Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;
using SqlAnalyzer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlAnalyzer.Analyzers
{
    public class DeleteUpdateWithWhere : SqlCodeObjectRecursiveVisitor, IAnalyzer
    {
        internal const string Message = "DELETE or UPDATE should have WHERE clause";
        private readonly List<SqlCodeObject> objects = new List<SqlCodeObject>();
        public string Name => "Delete or Update should have Where";

        public IEnumerable<DiagnosticMessage> Analyze(SqlScript script)
        {
            objects.Clear();
            Visit(script);
            foreach (var sqlObject in objects)
                yield return DiagnosticMessage.Warning(new Span(sqlObject.StartLocation.Offset, sqlObject.Length), Message);
        }

        public override void Visit(SqlDeleteSpecification codeObject)
        {
            if (codeObject.WhereClause == null)
                objects.Add(codeObject);
        }

        public override void Visit(SqlUpdateSpecification codeObject)
        {
            if (codeObject.WhereClause == null)
                objects.Add(codeObject);
        }
    }
}
