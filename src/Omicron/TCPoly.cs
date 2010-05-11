namespace Omicron
{
    public class TCPoly : ITypeConst
    {
        private IKind mKind;
        
        public IKind Kind
        {
            get { return mKind; }
        }
        
        public TCPoly(IKind kind)
        {
            mKind = kind;
        }
        
        public IType Eval(Unique unique, IType type)
        {
            return this;
        }
        
        public bool Equals(IType type)
        {
            if (!(type is TCPoly))
            {
                return false;
            }
            return mKind == ((TCPoly)type).mKind;
        }
        
        public IType Replace(Unique unique, IType type)
        {
            return this;
        }
        
        public IType Rename()
        {
            return this;
        }
        
        public string Show()
        {
            return string.Format("Poly[{0}]", mKind.Show());
        }
    }
}
