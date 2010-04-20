namespace Omicron
{
    public interface IValueCtxt
    {
        bool TryGetValue(string valueVarName, out IType result);
        void Add(string valueVarName, IType type);
        IValueCtxt MakeChild();
    }
}
