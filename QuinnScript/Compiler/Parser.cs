using System.Collections.Generic;
using System.Text;

using QuinnScript.Objects.Lines;

namespace QuinnScript.Compiler;

class Parser
{
    public readonly string FileName;
    public readonly string Source;

    private readonly List<AnonymousLine> _lines;
    private readonly List<string> _lineBuffer;
    private StringBuilder _buffer;

    private bool _quotationActive;

    private int _realLine;

    public Parser(string source, string filename)
    {
        this.FileName = filename;
        this.Source = source.Trim().Replace("\r", string.Empty).Replace("\t", string.Empty);

        this._lines = new List<AnonymousLine>();
        this._lineBuffer = new List<string>();
        this._buffer = new StringBuilder();

        this._quotationActive = false;

        this._realLine = 1;
    }

    public AnonymousLine[] ParseSource()
    {
        for (int i = 0; i < this.Source.Length; i++) {
            char c = this.Source[i];

            switch (c) {
                case '\n':
                    this._handleNewline();
                    break;

                case ';':
                    this._handleLineBreak();
                    break;

                case ' ':
                    this._handleTokenBreak(c);
                    break;

                case ',':
                    this._handleTokenBreak(c);
                    break;

                case '"':
                    this._handleQuotation();
                    break;

                case '\\':
                    this._handleBackslash(this.Source[++i]);
                    break;

                default:
                    _ = this._buffer.Append(c);
                    break;
            }
        }

        if (this._buffer.Length > 0)
            this._pushBuffer();

        if (this._lineBuffer.Count > 0)
            this._pushLineBuffer();

        return this._lines.ToArray();
    }

    private void _pushLineBuffer()
    {
        this._lines.Add(new AnonymousLine(this._lineBuffer.ToArray(), this.FileName, this._realLine));
        this._lineBuffer.Clear();
    }

    private void _pushBuffer()
    {
        this._lineBuffer.Add(this._buffer.ToString());
        this._buffer = this._buffer.Clear();
    }

    private void _handleNewline()
    {
        this._realLine++;

        if (this._quotationActive)
            return;

        if (this._buffer.Length > 0)
            this._pushBuffer();
    }

    private void _handleLineBreak()
    {
        if (this._quotationActive) {
            _ = this._buffer.Append(';');
            return;
        }

        if (this._buffer.Length > 0)
            this._pushBuffer();

        if (this._lineBuffer.Count > 0)
            this._pushLineBuffer();
    }

    private void _handleTokenBreak(char c)
    {
        if (this._quotationActive) {
            _ = this._buffer.Append(c);
            return;
        }

        if (this._buffer.Length > 0)
            this._pushBuffer();
    }

    private void _handleQuotation()
    {
        _ = this._buffer.Append('"');
        this._quotationActive = !this._quotationActive;
    }

    private void _handleBackslash(char next)
    {
        _ = this._buffer.Append(next switch
        {
            '\\' => '\\',
            'n' => '\n',
            't' => '\t',
            _ => next
        });
    }
}
