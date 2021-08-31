using Microsoft.VisualStudio.Text;
using SqlAnalyzer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlAnalyzerExtension
{
    class Error
    {
        public SnapshotSpan SnapshotSpan { get; }
        public DiagnosticMessage Message { get; }

        public Error(SnapshotSpan snapshotSpan, DiagnosticMessage message)
        {
            SnapshotSpan = snapshotSpan;
            Message = message;
        }

        public override bool Equals(object obj)
        {
            if (obj is Error other)
                return SnapshotSpan.Start.Position == other.SnapshotSpan.Start.Position && SnapshotSpan.End.Position == other.SnapshotSpan.End.Position && Message == other.Message;
            return false;
        }
    }
}
