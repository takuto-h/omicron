namespace Omicron
{
    public class TERec : ITypeExpr
    {
        private IKind mKind;
        
        public TERec(IKind kind)
        {
            mKind = kind;
        }
        
        public IKind Check(ITypeCtxt typeCtxt)
        {
            return new KArrow(
                new KArrow(mKind, KCType.Instance),
                KCType.Instance
            );
        }
        
        public IType Eval(ITypeEnv typeEnv)
        {
            return TCRec.Instance;
        }
        
        public string Show()
        {
            return string.Format("Rec[{0}]", mKind.Show());
        }
    }
}
