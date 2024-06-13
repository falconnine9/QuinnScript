using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using QuinnScript.Errors;

namespace QuinnScript.Compiler;

class Formatter
{
    public readonly string FileName;
    public readonly string Source;

    private int _line;
    private int _varNum;
    private List<string> _sourceLines;

    public Formatter(string s, string filename)
    {
        this.FileName = filename;
        this.Source = s;

        this._line = 0;
        this._varNum = 0;
        this._sourceLines = new List<string>();
    }

    public string FormatSource()
    {
        this._splitLines();
        
        for (int i = 0; i < _sourceLines.Count; i++) {
            this._line++;
            string line = _sourceLines[i];
            bool quotation = false;
            
            for (int j = 0; j < line.Length; j++) {
                char c = line[j];

                if (c == '"')
                    quotation = !quotation;

                if (c == '(' && !quotation) {
                    int start = j;
                    int end = this._highlightInline(line, start);

                    if (end == start + 1)
                        throw new InlineArgumentError("Empty inline arguments are not allowed", this.FileName, this._line);

                    string il = string.Format("set _v{0} {1}", this._varNum, line.Substring(start + 1, end - start - 1));
                    _sourceLines[i] = line
                        .Remove(start, end - start + 1)
                        .Insert(start, "$_v" + this._varNum.ToString());
                    _sourceLines.Insert(i, il);

                    this._varNum++;
                    i--;
                    break;
                }
            }
        }

        return string.Join(';', this._sourceLines);
    }

    private int _highlightInline(string s, int index)
    {
        bool quotation = false;
        int depth = 0;
        for (int i = index + 1; i < s.Length; i++) {
            char c = s[i];

            if (c == '"') {
                quotation = !quotation;
                continue;
            }

            if (quotation)
                continue;

            if (c == ')') {
                if (depth > 0)
                    depth--;
                else
                    return i;
            }
            else if (c == '(')
                depth++;
        }
        throw new InlineArgumentError("Unclosed inline argument before line break", this.FileName, this._line);
    }

    private void _splitLines()
    {
        bool quotation = false;
        string buffer = "";
        for (int i = 0; i < this.Source.Length; i++) {
            char c = this.Source[i];

            if (c == ';') {
                this._sourceLines.Add(buffer);
                buffer = "";
            }
            else if (c == '"') {
                quotation = !quotation;
                buffer += c;
            }
            else
                buffer += c;
        }
        if (buffer.Length > 0)
            _sourceLines.Add(buffer);
    }
}
