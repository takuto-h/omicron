namespace Omicron
{
    public class TEAbs : ITypeExpr
    {
        private string mTypeVarName;
        private IKind mKind;
        private ITypeExpr mTypeExpr;
        
        public IKind ParameterKind
        {
            get { return mKind; }
        }
        
        public TEAbs(
            string typeVarName,
            IKind kind,
            ITypeExpr typeExpr
        )
        {
            mTypeVarName = typeVarName;
            mKind = kind;
            mTypeExpr = typeExpr;
        }
        
        public IKind Check(ITypeCtxt typeCtxt)
        {
            typeCtxt = typeCtxt.MakeChild();
            typeCtxt.Add(mTypeVarName, mKind);
            IKind kind = mTypeExpr.Check(typeCtxt);
            return new KArrow(mKind, kind);
        }
        
        public IType Eval(ITypeEnv typeEnv)
        {
            Unique unique = new Unique();
            typeEnv = typeEnv.MakeChild();
            typeEnv.Add(mTypeVarName, new TVar(unique));
            IType type = mTypeExpr.Eval(typeEnv);
            return new TAbs(unique, type);
        }
        
        public string Show()
        {
            return string.Format(
                "^{0}:{1}{{{2}}}", mTypeVarName, mKind.Show(), mTypeExpr.Show()
            );
        }
    }
}
