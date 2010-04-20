namespace Omicron
{
    public interface IValueFunc : IValue
    {
        IValue Apply(IValue value);
    }
}
