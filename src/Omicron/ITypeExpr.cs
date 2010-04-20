namespace Omicron
{
    public interface ITypeExpr : IShowable
    {
        IKind Check(ITypeCtxt typeCtxt);
        IType Eval(ITypeEnv typeEnv);
    }
}
