using Rubberduck.Model.Abstract;

namespace Rubberduck.Model
{
    /// <summary>
    /// Encapsulates a request to indent some code.
    /// </summary>
    //[XmlRoot("IndenterViewModel")]
    public class IndenterViewModel : IIndenterSettings
    {
        /// <summary>
        /// The assembly version for Rubberduck.SmartIndenter.dll
        /// </summary>
        public string IndenterVersion { get; set; }

        /// <summary>
        /// The code to indent. Expects <c>\r\n</c> line endings.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// If true, forces all Debug.Print to appear in column 1 regardless of indent.
        /// </summary>
        public bool ForceDebugPrintInColumn1 { get; set; }
        /// <summary>
        /// If true, ensures consistent vertical spacing between procedures; controlled by <see cref="LinesBetweenProcedures"/>.
        /// </summary>
        public bool VerticallySpaceProcedures { get; set; } = true;
        /// <summary>
        /// The number of spaces per indent step.
        /// </summary>
        /// <remarks>Default value is 4.</remarks>
        public int IndentSpaces { get; set; } = 4;
        /// <summary>
        /// The column to align end-of-line comments at.
        /// </summary>
        public int EndOfLineCommentColumnSpaceAlignment { get; set; } = 1;
        /// <summary>
        /// Controls whether empty lines are ignored (left as-is), removed, or indented (default).
        /// </summary>
        public IndenterEmptyLineHandling EmptyLineHandlingMethod { get; set; } = IndenterEmptyLineHandling.Indent;
        public int EmptyLineHandlingMethodValue { get => (int)EmptyLineHandlingMethod; set => EmptyLineHandlingMethod = (IndenterEmptyLineHandling)value; }
        /// <summary>
        /// Controls how end-of-line comments are indented; absolute, same-gap, standard gap, or aligned in a column.
        /// </summary>
        public IndenterEndOfLineCommentStyle EndOfLineCommentStyle { get; set; } = IndenterEndOfLineCommentStyle.Absolute;
        public int EndOfLineCommentStyleValue { get => (int)EndOfLineCommentStyle; set => EndOfLineCommentStyle = (IndenterEndOfLineCommentStyle)value; }
        /// <summary>
        /// The column to align <c>Dim</c> statements at.
        /// </summary>
        public int AlignDimColumn { get; set; } = 1;
        /// <summary>
        /// Whether to align <c>Dim</c> statements at a particular column, controlled by <see cref="AlignDimColumn"/>.
        /// </summary>
        public bool AlignDims { get; set; }
        /// <summary>
        /// If true, precompiler directives will be indented.
        /// </summary>
        public bool IndentCompilerDirectives { get; set; }
        /// <summary>
        /// If true, precompiler directives will be forced to column 1.
        /// </summary>
        public bool ForceCompilerDirectivesInColumn1 { get; set; } = true;
        /// <summary>
        /// If true, <c>Stop</c> statements will be forced to column 1.
        /// </summary>
        public bool ForceStopInColumn1 { get; set; }
        /// <summary>
        /// If true, <c>Debug.Assert</c> statements will be forced to column 1.
        /// </summary>
        public bool ForceDebugAssertInColumn1 { get; set; }
        /// <summary>
        /// If true, same-name property members will be consistently regrouped.
        /// </summary>
        public bool GroupRelatedProperties { get; set; }
        /// <summary>
        /// If true, <c>Debug</c> statements will be forced to column 1.
        /// </summary>
        public bool ForceDebugStatementsInColumn1 { get; set; }
        /// <summary>
        /// If true, <c>Case</c> statements will be indented one level under the corresponding <c>Select Case</c> statement.
        /// </summary>
        public bool IndentCase { get; set; } = true;
        /// <summary>
        /// If true, operators will be ignored when indenting line-continuated statements.
        /// </summary>
        public bool IgnoreOperatorsInContinuations { get; set; }
        /// <summary>
        /// If true, line continuations wil be aligned.
        /// </summary>
        public bool AlignContinuations { get; set; } = true;
        /// <summary>
        /// If true, comments will be aligned with the code.
        /// </summary>
        public bool AlignCommentsWithCode { get; set; } = true;
        /// <summary>
        /// If true, empty line in the first block of a procedure will be ignored.
        /// </summary>
        public bool IgnoreEmptyLinesInFirstBlocks { get; set; }
        /// <summary>
        /// If true, a block of declarations at the top of a procedure will be indented.
        /// </summary>
        public bool IndentFirstDeclarationBlock { get; set; }
        /// <summary>
        /// If true, the first comments block of a procedure will be indented.
        /// </summary>
        public bool IndentFirstCommentBlock { get; set; }
        /// <summary>
        /// If true, <c>Enum</c> members will be indented.
        /// </summary>
        public bool IndentEnumTypeAsProcedure { get; set; } = true;
        /// <summary>
        /// If true, procedure scopes add an indent level.
        /// </summary>
        public bool IndentEntireProcedureBody { get; set; } = true;
        /// <summary>
        /// The number of empty lines between procedures.
        /// </summary>
        public int LinesBetweenProcedures { get; set; } = 1;
    }
}
