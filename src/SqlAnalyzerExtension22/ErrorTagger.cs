using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Adornments;
using Microsoft.VisualStudio.Text.Tagging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlAnalyzerExtension
{
    public class ErrorTagger : ITagger<IErrorTag>
    {
        private readonly Analyzer analyzer;
        private List<Error> errors = new List<Error>();
        internal IEnumerable<Error> Errors => errors;

        public  ErrorTagger(Analyzer analyzer)
        {
            this.analyzer = analyzer;
            analyzer.AddTaggerAsync(this);
        }

        public event EventHandler<SnapshotSpanEventArgs> TagsChanged;

        public IEnumerable<ITagSpan<IErrorTag>> GetTags(NormalizedSnapshotSpanCollection spans)
        {
            foreach(var error in errors)
            {
                if (spans.IntersectsWith(error.SnapshotSpan))
                    yield return new TagSpan<IErrorTag>(
                        error.SnapshotSpan, new ErrorTag(GetErrorType(error), error.Message.Message));
            }
        }

        private string GetErrorType(Error error)
        {
            switch(error.Message.Severity)
            {
                case SqlAnalyzer.DTO.Severity.Error: return PredefinedErrorTypeNames.OtherError;
                case SqlAnalyzer.DTO.Severity.Warning: return PredefinedErrorTypeNames.Warning;
                case SqlAnalyzer.DTO.Severity.Suggestion: return PredefinedErrorTypeNames.Suggestion;
            }
            return PredefinedErrorTypeNames.HintedSuggestion;
        }

        internal void UpdateErrors(ITextSnapshot snapshot, IEnumerable<Error> errors)
        {
            if (errors == null)
            {
                ClearErrors(snapshot);
                return;
            }
            List<Error> newErrors = new List<Error>();
            int oldErrorsCount = 0;
            int newErrorsCount = 0;
            foreach(var error in errors)
            {
                if (this.errors.Contains(error))
                {
                    newErrors.Add(error);
                    oldErrorsCount++;
                }
                else
                {
                    newErrors.Add(error);
                    newErrorsCount++;
                }
            }
            if (oldErrorsCount != this.errors.Count || newErrorsCount > 0)
            {
                this.errors = newErrors;
                TagsChanged?.Invoke(this, new SnapshotSpanEventArgs(new SnapshotSpan(snapshot, this.errors.First().SnapshotSpan.Span)));
            }
        }

        internal void ClearErrors(ITextSnapshot currentSnapshot)
        {
            this.errors.Clear();
            TagsChanged?.Invoke(this, new SnapshotSpanEventArgs(new SnapshotSpan(currentSnapshot, Span.FromBounds(0, 0))));
        }
    }
}
