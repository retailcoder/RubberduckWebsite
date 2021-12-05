# RubberduckServices  
A library that references Rubberduck libraries and exposes indenter and syntax highlighter services.

The **\Libs** folder contains compiled binaries for certain specific Rubberduck libraries:

 - Rubberduck.Parsing.dll
 - Rubberduck.SmartIndenter.dll
 - Rubberduck.VBEditor.dll

These files should be overwritten at every Rubberduck release of the [main] branch.

## ISyntaxHighlighterService  
This service exposes a `FormatAsync(string) : Task<string>` method that takes a string containing VBA code, tokenizes it, and returns the code string with `&nbsp;` indents and CSS classes for syntax highlighting.

## IIndenterService  
This service exposes an `IndentAsync(string, IIndenterSettings) : Task<string[]>` method that takes a string containing VBA code along with indenter options, and returns each indented line of code in an array of strings.

In order to avoid requiring referencing projects to also reference Rubberduck libraries, this service wraps the `IIndenterSettings` interface and the `EndOfLineCommentStyle` and `EmptyLineHandling` enum types with its own types; changes in these `Rubberduck.SmartIndenter` types should be reflected in this service.