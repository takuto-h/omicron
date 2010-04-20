namespace Omicron
{
    public class VETypeAbs : IValueExpr
    {
        private string mTypeVarName;
        private IKind mKind;
        private IValueExpr mValueExpr;
        
        public VETypeAbs(
            string typeVarName,
            IKind kind,
            IValueExpr valueExpr
        )
        {
            mTypeVarName = typeVarName;
            mKind = kind;
            mValueExpr = valueExpr;
        }
        
        public IType Check(
            ITypeCtxt typeCtxt,
            ITypeEnv typeEnv,
            IValueCtxt valueCtxt
        )
        {
            Unique unique = new Unique();
            typeCtxt = typeCtxt.MakeChild();
            typeEnv = typeEnv.MakeChild();
            typeCtxt.Add(mTypeVarName, mKind);
            typeEnv.Add(mTypeVarName, new TVar(unique));
            IType type = mValueExpr.Check(typeCtxt, typeEnv, valueCtxt);
            return MakePolymorphicType(unique, mKind, type);
        }
        
        public static IType MakePolymorphicType(
            Unique unique, IKind kind, IType type
        )
        {
            return new TApp(new TCPoly(kind), new TAbs(unique, type));
        }
        
        public IValue Eval(IValueEnv valueEnv)
        {
            return mValueExpr.Eval(valueEnv);
        }
        
        public string Show()
        {
            return string.Format(
                "~{0}:{1}{{{2}}}", mTypeVarName, mKind.Show(), mValueExpr.Show()
            );
        }
    }
}
