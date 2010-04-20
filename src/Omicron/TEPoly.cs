namespace Omicron
{
    public class TEPoly : ITypeExpr
    {
        private IKind mKind;
        
        public TEPoly(IKind kind)
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
            return new TCPoly(mKind);
        }
        
        public string Show()
        {
            return string.Format("Poly[{0}]", mKind.Show());
        }
    }
}
