using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlAnalyzer.DTO
{
    public class Span
    {
        public Span(int from, int length)
        {
            From = from;
            Length = length;
        }

        public int From { get; }
        public int Length { get; }
    }
}
