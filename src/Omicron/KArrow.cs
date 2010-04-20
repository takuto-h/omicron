using System;

namespace Omicron
{
    public class KArrow : IKind
    {
        private IKind mKind1;
        private IKind mKind2;
        
        public IKind Left
        {
            get { return mKind1; }
        }
        
        public IKind Right
        {
            get { return mKind2; }
        }
        
        public KArrow(IKind kind1, IKind kind2)
        {
            mKind1 = kind1;
            mKind2 = kind2;
        }
        
        public bool Equals(IKind kind)
        {
            if (!(kind is KArrow))
            {
                return false;
            }
            IKind kind1 = ((KArrow)kind).mKind1;
            IKind kind2 = ((KArrow)kind).mKind2;
            return mKind1.Equals(kind1) && mKind2.Equals(kind2);
        }
        
        public string Show()
        {
            return string.Format("({0} -> {1})", mKind1.Show(), mKind2.Show());
        }
    }
}
