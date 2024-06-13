using QuinnScript.Objects.Types;

namespace QuinnScript.VM;

class VariableConstants
{
    public static readonly string[] ConstVars = {
        "math.pi",
        "math.e"
    };

    public static TypeBase GetValue(string var) => var switch
    {
        "math.pi" => new NumberType(3.141592653589793m),
        "math.e" => new NumberType(2.718281828459045m),
        _ => null
    };

    public static void SetConstantVariables()
    {
        foreach (string var in ConstVars)
            Executor.Variables.Add(var, new Variable(GetValue(var), true));
    }
}
