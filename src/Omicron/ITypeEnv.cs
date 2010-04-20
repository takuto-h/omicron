namespace Omicron
{
    public interface ITypeEnv
    {
        bool TryGetValue(string typeVarName, out IType result);
        void Add(string typeVarName, IType type);
        ITypeEnv MakeChild();
    }
}
