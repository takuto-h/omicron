namespace Omicron
{
    public interface ITypeFunc : IType
    {
        IType Apply(IType type);
    }
}
