﻿using System.Collections.Generic;

using QuinnScript.Errors;
using QuinnScript.Objects;
using QuinnScript.Objects.Arguments;
using QuinnScript.Objects.Lines;
using QuinnScript.Objects.Types;

namespace QuinnScript.Compiler;

class Evaluator
{
    public static Executable SummarizeLines(string file, AnonymousLine[] source)
    {
        List<LineBase> lines = new();

        foreach (AnonymousLine line in source) {
            string variable = null;
            bool is_const = false;
            int start_pos = 0;

            if (line.Arguments.Length == 0)
                continue;

            if (line.Arguments[0] == "set") {
                int variable_pos = 1;

                if (line.Arguments.Length == 1)
                    throw new ExpectedTokenError("Expected variable name after set keyword", line.OriginFile, line.OriginLine);

                if (line.Arguments[1] == "const") {
                    is_const = true;
                    variable_pos = 2;
                }

                if (!Algorithms.IsValidVariable(line.Arguments[variable_pos]))
                    throw new InvalidVariableError("Cannot set variable to instruction or keyword", line.OriginFile, line.OriginLine);

                variable = line.Arguments[variable_pos];
                start_pos = 2 + (variable_pos - 1);
            }

            List<ArgumentBase> args = new();
            for (int i = start_pos + 1; i < line.Arguments.Length; i++) {
                ArgumentBase result = TokenizeArgument(line.Arguments[i]);
                if (result is null)
                    throw new UndefinedReferenceError("Undefined reference \"" + line.Arguments[i] + "\" in argument list", line.OriginFile, line.OriginLine);

                args.Add(result);
            }

            if (line.Arguments.Length < start_pos + 1)
                throw new ExpectedTokenError("No instruction or constant on non whitespace line", line.OriginFile, line.OriginLine);

            ArgumentBase initial_result = TokenizeArgument(line.Arguments[start_pos]);
            if (initial_result is null) {
                if (!Algorithms.IsValidInstruction(line.Arguments[start_pos]))
                    throw new UndefinedReferenceError("Undefined instruction referenced", line.OriginFile, line.OriginLine);

                lines.Add(new ExecutableLine(
                    line.Arguments[start_pos],
                    args.ToArray(),
                    variable is null ? null : new VariableSet(variable, is_const),
                    line.OriginLine
                ));
            }
            else {
                if (args.Count > 0)
                    throw new ArgumentError("Too many arguments given in constant line", line.OriginFile, line.OriginLine);

                lines.Add(new ConstantLine(
                    initial_result,
                    variable is null ? null : new VariableSet(variable, is_const),
                    line.OriginLine
                ));
            }
        }

        return new Executable(file, lines.ToArray());
    }

    public static ArgumentBase TokenizeArgument(string value)
    {
        if (value.StartsWith('"') && value.EndsWith('"'))
            return new TypeArgument(new StringType(value[1..^1]));

        else if (decimal.TryParse(value, out decimal result))
            return new TypeArgument(new NumberType(result));

        else if (value is "true" or "false")
            return new TypeArgument(new BooleanType(value == "true"));

        else if (value.StartsWith('$'))
            return new VariableArgument(value[1..]);

        else
            return null;
    }
}
