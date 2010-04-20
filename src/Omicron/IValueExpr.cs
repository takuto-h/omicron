namespace Omicron
{
    public interface IValueExpr : IShowable
    {
        IType Check(
            ITypeCtxt typeCtxt,
            ITypeEnv typeEnv,
            IValueCtxt valueCtxt
        );
        IValue Eval(IValueEnv valueEnv);
    }
}
