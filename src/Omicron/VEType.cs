namespace Omicron
{
    public class VEType : IValueExpr
    {
        private ITypeExpr mTypeExpr;
        
        public VEType(ITypeExpr typeExpr)
        {
            mTypeExpr = typeExpr;
        }
        
        public IType Check(
            ITypeCtxt typeCtxt,
            ITypeEnv typeEnv,
            IValueCtxt valueCtxt
        )
        {
            mTypeExpr.Check(typeCtxt);
            mTypeExpr.Eval(typeEnv);
            return TCUnit.Instance;
        }
        
        public IValue Eval(IValueEnv valueEnv)
        {
            return VCUnit.Instance;
        }
        
        public string Show()
        {
            return string.Format("type {0}", mTypeExpr.Show());
        }
    }
}
