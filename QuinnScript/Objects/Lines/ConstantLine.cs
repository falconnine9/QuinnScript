using System;

using QuinnScript.Objects.Arguments;
using QuinnScript.Objects.Types;

namespace QuinnScript.Objects.Lines;

class ConstantLine : LineBase
{
    public override Type RealType { get; }
    public override int OriginLine { get; }
    public VariableSet ReturnVariable { get; }
    public ArgumentBase Constant { get; }

    public ConstantLine(ArgumentBase value, VariableSet ret_var, int origin_line=default)
    {
        this.RealType = typeof(ConstantLine);
        this.OriginLine = origin_line;
        this.ReturnVariable = ret_var;
        this.Constant = value;
    }
}
