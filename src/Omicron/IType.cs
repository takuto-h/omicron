using System;

namespace Omicron
{
    public interface IType : IShowable, IEquatable<IType>
    {
        IType Replace(Unique unique, IType type);
        IType Rename();
        IType Eval(Unique unique, IType type);
    }
}
