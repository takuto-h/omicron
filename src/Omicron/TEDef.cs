namespace Omicron
{
    public class TEDef : ITypeExpr
    {
        private string mTypeVarName;
        private ITypeExpr mTypeExpr;
        
        public TEDef(string typeVarName, ITypeExpr typeExpr)
        {
            mTypeVarName = typeVarName;
            mTypeExpr = typeExpr;
        }
        
        public IKind Check(ITypeCtxt typeCtxt)
        {
            IKind kind = mTypeExpr.Check(typeCtxt);
            typeCtxt.Add(mTypeVarName, kind);
            return kind;
        }
        
        public IType Eval(ITypeEnv typeEnv)
        {
            IType type = mTypeExpr.Eval(typeEnv);
            typeEnv.Add(mTypeVarName, type);
            return type;
        }
        
        public string Show()
        {
            return string.Format(
                "def {0} = {1}", mTypeVarName, mTypeExpr.Show()
            );
        }
    }
}
