using System;

using QuinnScript.Objects.Types;

namespace QuinnScript.Objects.Arguments;

class TypeArgument : ArgumentBase
{
    public override Type RealType { get; }
    public TypeBase Value { get; }

    public TypeArgument(TypeBase value)
    {
        this.RealType = typeof(TypeArgument);
        this.Value = value;
    }
}
