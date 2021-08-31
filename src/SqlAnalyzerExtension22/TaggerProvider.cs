using Microsoft.VisualStudio.Shell.TableManager;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlAnalyzerExtension
{
    [Export(typeof(IViewTaggerProvider))]
    [TagType(typeof(IErrorTag))]
    [ContentType("text")]
    [TextViewRole(PredefinedTextViewRoles.Document)]
    [TextViewRole(PredefinedTextViewRoles.Analyzable)]
    public sealed class TaggerProvider : IViewTaggerProvider
    {
        [Import]
        ITextDocumentFactoryService textDocumentFactoryService;
        
        public string SourceTypeIdentifier => StandardTableDataSources.ErrorTableDataSource;

        public string Identifier => "SQLAnalyzer";

        public string DisplayName => "SQL Analyzer";

        [ImportingConstructor]
        internal TaggerProvider()
        {
        }

        public ITagger<T> CreateTagger<T>(ITextView textView, ITextBuffer buffer) where T : ITag
        {
            ITagger<T> tagger = null;

            if ((buffer == textView.TextBuffer) && (typeof(T) == typeof(IErrorTag)))
            {
                var analyzer = buffer.Properties.GetOrCreateSingletonProperty(typeof(Analyzer), () => new Analyzer(textView, buffer, textDocumentFactoryService));
                if (analyzer.Tagger == null)
                    tagger = new ErrorTagger(analyzer) as ITagger<T>;
                else
                    tagger = analyzer.Tagger as ITagger<T>;
            }

            return tagger;
        }
    }
}
