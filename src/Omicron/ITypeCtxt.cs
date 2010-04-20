namespace Omicron
{
    public interface ITypeCtxt
    {
        bool TryGetValue(string typeVarName, out IKind result);
        void Add(string typeVarName, IKind kind);
        ITypeCtxt MakeChild();
    }
}
