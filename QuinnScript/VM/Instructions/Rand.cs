using System;

using QuinnScript.Errors;
using QuinnScript.Objects.Arguments;
using QuinnScript.Objects.Lines;
using QuinnScript.Objects.Types;

namespace QuinnScript.VM.Instructions;

class Rand
{
    public static TypeBase RandInt(ExecutableLine ln)
    {
        int min;
        int max;
        try {
            min = (int)((ln.Arguments[0] as TypeArgument).Value as NumberType).Value;
            max = (int)((ln.Arguments[1] as TypeArgument).Value as NumberType).Value;
        }
        catch (NullReferenceException) {
            throw new ArgumentError("Randint instruction takes (number, number) arguments", Executor.Code.Name, ln.OriginLine);
        }

        return new NumberType(Executor.RandomGenerator.Next(min, max));
    }
}
