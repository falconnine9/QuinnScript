using QuinnScript.Errors;
using QuinnScript.Objects.Arguments;
using QuinnScript.Objects.Lines;
using QuinnScript.Objects.Types;
using System;

namespace QuinnScript.VM.Instructions;

class MathUtil
{
    public static TypeBase Abs(ExecutableLine ln)
    {
        decimal n;
        try {
            n = ((ln.Arguments[0] as TypeArgument).Value as NumberType).Value;
        }
        catch (NullReferenceException) {
            throw new ArgumentError("Abs instruction takes (number) arguments", Executor.Code.Name, ln.OriginLine);
        }

        return new NumberType((decimal)Math.Abs((double)n));
    }

    public static TypeBase Sqrt(ExecutableLine ln)
    {
        decimal n;
        try {
            n = ((ln.Arguments[0] as TypeArgument).Value as NumberType).Value;
        }
        catch (NullReferenceException) {
            throw new ArgumentError("Sqrt instruction takes (number) arguments", Executor.Code.Name, ln.OriginLine);
        }

        return new NumberType((decimal)Math.Sqrt((double)n));
    }

    public static TypeBase Log(ExecutableLine ln)
    {
        decimal b;
        decimal n;
        try {
            b = ((ln.Arguments[0] as TypeArgument).Value as NumberType).Value;
            n = ((ln.Arguments[1] as TypeArgument).Value as NumberType).Value;
        }
        catch (NullReferenceException) {
            throw new ArgumentError("Log instruction takes (number, number) arguments", Executor.Code.Name, ln.OriginLine);
        }

        return new NumberType((decimal)(Math.Log((double)n) / Math.Log((double)b)));
    }

    public static TypeBase Sin(ExecutableLine ln)
    {
        decimal n;
        try {
            n = ((ln.Arguments[0] as TypeArgument).Value as NumberType).Value;
        }
        catch (NullReferenceException) {
            throw new ArgumentError("Sin instruction takes (number) arguments", Executor.Code.Name, ln.OriginLine);
        }

        return new NumberType((decimal)Math.Sin((double)n));
    }

    public static TypeBase Cos(ExecutableLine ln)
    {
        decimal n;
        try {
            n = ((ln.Arguments[0] as TypeArgument).Value as NumberType).Value;
        }
        catch (NullReferenceException) {
            throw new ArgumentError("Cos instruction takes (number) arguments", Executor.Code.Name, ln.OriginLine);
        }

        return new NumberType((decimal)Math.Cos((double)n));
    }
}
