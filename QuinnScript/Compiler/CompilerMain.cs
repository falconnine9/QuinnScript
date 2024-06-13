using QuinnScript.Objects;
using QuinnScript.Objects.Lines;

namespace QuinnScript.Compiler;

class CompilerMain
{
    public static Executable CompileSource(string filename, string source)
    {
        // Formats the raw source into code that can be parsed
        Formatter fmt = new(source, filename);
        string formatted = fmt.FormatSource();

        // Parses the code into a table of raw lines
        Parser fp = new(formatted, filename);
        AnonymousLine[] table = fp.ParseSource();

        // Evaluates each line into an executable statement
        Executable code = Evaluator.SummarizeLines(filename, table);

        // Creates the index tables for labels and block statements
        code.LabelIndex = Indexers.IndexLabels(code);
        code.BlockIndex = Indexers.IndexBlocks(code);

        code.Name = filename;
        return code;
    }
}
