﻿using System;
using System.Collections.Generic;
using System.Linq;

using QuinnScript.Errors;
using QuinnScript.Objects;
using QuinnScript.Objects.Arguments;
using QuinnScript.Objects.Blocks;
using QuinnScript.Objects.Lines;
using QuinnScript.Objects.Types;

namespace QuinnScript.Compiler;

class Indexers
{
    public static Dictionary<string, int> IndexLabels(Executable source)
    {
        Dictionary<string, int> index = new();

        for (int i = 0; i < source.Lines.Length; i++) {
            LineBase ln = source.Lines[i];
            if (ln.RealType != typeof(ExecutableLine))
                continue;

            var real = ln as ExecutableLine;
            if (real.Instruction != "label")
                continue;
            if (real.ReturnVariable is not null)
                throw new InvalidVariableError("Cannot set a variable at this location", source.Name, real.OriginLine);

            if (real.Arguments.Length != 1)
                throw new ArgumentError("Label instruction requires 1 argument", source.Name, real.OriginLine);

            string label_name;
            try {
                label_name = ((real.Arguments[0] as TypeArgument).Value as StringType).Value;
            }
            catch (NullReferenceException) {
                throw new ArgumentError("Label instruction requires (string) arguments", source.Name, real.OriginLine);
            }

            index.Add(label_name, i);
        }

        return index;
    }

    public static Dictionary<int, BlockBase> IndexBlocks(Executable source)
    {
        Dictionary<int, BlockBase> index = new();

        for (int i = 0; i < source.Lines.Length; i++) {
            LineBase ln = source.Lines[i];
            if (ln.RealType != typeof(ExecutableLine))
                continue;

            var real = ln as ExecutableLine;
            if (!Constants.BlockInstructions.Contains(real.Instruction))
                continue;

            switch (real.Instruction) {
                case "if":
                    if (real.ReturnVariable is not null)
                        throw new InvalidVariableError("Cannot set a variable at this location", source.Name, real.OriginLine);
                    index.Add(i, new IfBlock(i, _findEndPosition(source, real, i)));
                    break;

                case "loop":
                    if (real.ReturnVariable is null)
                        throw new ExpectedTokenError("Must set a variable at this location", source.Name, real.OriginLine);
                    index.Add(i, new LoopBlock(i, _findEndPosition(source, real, i)));
                    break;
            }
        }

        return index;
    }

    private static int _findEndPosition(Executable src, ExecutableLine ln, int cond_pos)
    {
        int depth = 0;
        int pos = -1;
        for (int i = cond_pos + 1; i < src.Lines.Length; i++) {
            LineBase inner_ln = src.Lines[i];
            if (inner_ln.RealType != typeof(ExecutableLine))
                continue;

            var real = inner_ln as ExecutableLine;
            if (Constants.BlockInstructions.Contains(real.Instruction))
                depth++;
            else if (real.Instruction == "end") {
                if (depth > 0)
                    depth--;
                else {
                    pos = i;
                    break;
                }
            }
        }

        return pos == -1
            ? throw new ExpectedTokenError("Block instruction found without matching endpoint", src.Name, ln.OriginLine)
            : pos;
    }
}
