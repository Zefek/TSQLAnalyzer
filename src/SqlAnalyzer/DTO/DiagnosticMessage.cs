﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlAnalyzer.DTO
{
    public enum Severity
    {
        Error,
        Warning,
        Suggestion
    }
    public class DiagnosticMessage
    {
        internal DiagnosticMessage(Span span, string message, Severity severity)
        {
            Span = span;
            Message = message;
            Severity = severity;
        }

        public Span Span { get; }
        public string Message { get; }
        public Severity Severity { get; }

        public static DiagnosticMessage Warning(Span span, string message) => new DiagnosticMessage(span, message, Severity.Warning);
    }
}
