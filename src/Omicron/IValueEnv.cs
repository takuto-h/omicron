namespace Omicron
{
    public interface IValueEnv
    {
        bool TryGetValue(string valueVarName, out IValue result);
        void Add(string valueVarName, IValue value);
        IValueEnv MakeChild();
    }
}
