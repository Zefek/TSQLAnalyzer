using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using SqlAnalyzer;
using SqlAnalyzer.DTO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SqlAnalyzerExtension
{
    public class Analyzer
    {
        private readonly ITextView textView;
        private readonly ITextBuffer buffer;
        private bool isAnalyzed = false;
        private readonly bool canBeAnalyzed = true;

        public ErrorTagger Tagger { get; private set; } = null;

        public Analyzer(ITextView textView, ITextBuffer buffer, ITextDocumentFactoryService textDocumentFactoryService)
        {
            this.textView = textView;
            this.buffer = buffer;
            if(textDocumentFactoryService.TryGetTextDocument(buffer, out var document))
            {
                System.IO.FileInfo fi = new System.IO.FileInfo(document.FilePath);
                canBeAnalyzed = fi.Extension.ToLower() == ".sql";
            }
        }

        internal async Task AddTaggerAsync(ErrorTagger tagger)
        {
            if (this.Tagger == null)
            {
                Tagger = tagger;
                buffer.Changed += Buffer_Changed;
                await AnalyzeAsync();
            }
        }

        private async void Buffer_Changed(object sender, TextContentChangedEventArgs e)
        {
            await AnalyzeAsync();
        }

        private async Task AnalyzeAsync()
        {
            if (!isAnalyzed && canBeAnalyzed)
            {
                isAnalyzed = true;
                string text = textView.TextBuffer.CurrentSnapshot.GetText();
                IEnumerable<DiagnosticMessage> analyzeResult = await Task.Run(() =>
                {
                    var sqlAnalyzers = new SqlAnalyzerService();
                    return sqlAnalyzers.Analyze(text);
                })
                .ConfigureAwait(true);

                if (analyzeResult.Any())
                {
                    List<Error> errors = new List<Error>();
                    foreach (var message in analyzeResult)
                    {
                        errors.Add(
                            new Error(
                                new SnapshotSpan(buffer.CurrentSnapshot, message.Span.From, message.Span.Length), message));
                    }
                    Tagger.UpdateErrors(buffer.CurrentSnapshot, errors);
                }
                else
                {
                    Tagger.ClearErrors(buffer.CurrentSnapshot);
                }
                isAnalyzed = false;
            }
        }
    }
}
