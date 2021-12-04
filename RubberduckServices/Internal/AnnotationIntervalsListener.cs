using Antlr4.Runtime.Misc;
using Rubberduck.Parsing.Grammar;

namespace RubberduckServices.Internal
{
    internal class AnnotationIntervalsListener : IntervalListener
    {
        public const string DefaultAnnotationClass = "annotation";

        public AnnotationIntervalsListener(string cssClass = DefaultAnnotationClass) : base(cssClass) { }

        public override void ExitAnnotationList(VBAParser.AnnotationListContext context)
        {
            // exclude the line-ending token at the end
            AddInterval(new Interval(context.SourceInterval.a, context.SourceInterval.b - 1));
        }
    }
}
