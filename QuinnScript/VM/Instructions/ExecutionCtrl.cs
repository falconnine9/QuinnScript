﻿using System;
using System.Threading;

using QuinnScript.Errors;
using QuinnScript.Objects.Arguments;
using QuinnScript.Objects.Lines;
using QuinnScript.Objects.Types;

namespace QuinnScript.VM.Instructions;

class ExecutionCtrl
{
    public static TypeBase Exit(ExecutableLine _)
    {
        Executor.ExecutionLine = Executor.Code.Lines.Length;
        return null;
    }

    public static TypeBase Jump(ExecutableLine ln)
    {
        string label;
        try {
            label = ((ln.Arguments[0] as TypeArgument).Value as StringType).Value;
        }
        catch (NullReferenceException) {
            throw new ArgumentError("Jump instruction takes (string) arguments", Executor.Code.Name, ln.OriginLine);
        }

        Executor.ExecutionLine = Executor.Code.LabelIndex.TryGetValue(label, out int result)
            ? result
            : throw new UndefinedReferenceError("Unknown label referenced", Executor.Code.Name, ln.OriginLine);

        return null;
    }

    public static TypeBase Throw(ExecutableLine ln)
    {
        string err_type;
        string message;
        try {
            err_type = ((ln.Arguments[0] as TypeArgument).Value as StringType).Value;
            message = ((ln.Arguments[1] as TypeArgument).Value as StringType).Value;
        }
        catch (NullReferenceException) {
            throw new ArgumentError("Throw instruction takes (string, string) arguments", Executor.Code.Name, ln.OriginLine);
        }

        throw new CustomError(err_type, message, Executor.Code.Name, ln.OriginLine);
    }

    public static TypeBase Wait(ExecutableLine ln)
    {
        decimal time;
        try {
            time = ((ln.Arguments[0] as TypeArgument).Value as NumberType).Value;
        }
        catch (NullReferenceException) {
            throw new ArgumentError("Wait instruction takes (decimal) arguments", Executor.Code.Name, ln.OriginLine);
        }

        if (time < 0)
            throw new ValueError("Wait time must be >=0", Executor.Code.Name, ln.OriginLine);

        Thread.Sleep((int)time);
        return null;
    }
}
