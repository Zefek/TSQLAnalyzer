using Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;
using Microsoft.SqlServer.Management.SqlParser.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlAnalyzer
{
    static class SqlParser
    {
        internal static SqlScript Parse(string sql)
        {
            return Parser.Parse(sql).Script;
        }
    }
}
