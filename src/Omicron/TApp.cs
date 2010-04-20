namespace Omicron
{
    public class TApp : IType
    {
        private IType mType1;
        private IType mType2;
        
        public IType Function
        {
            get { return mType1; }
        }
        
        public IType Argument
        {
            get { return mType2; }
        }
        
        public TApp(IType type1, IType type2)
        {
            mType1 = type1;
            mType2 = type2;
        }
        
        public IType Eval(Unique unique, IType type)
        {
            IType type1 = mType1.Eval(unique, type);
            IType type2 = mType2.Eval(unique, type);
            if (type1 is ITypeFunc)
            {
                return ((ITypeFunc)type1).Apply(type2);
            }
            else
            {
                return new TApp(type1, type2);
            }
        }
        
        public bool Equals(IType type)
        {
            if (!(type is TApp))
            {
                return false;
            }
            IType type1 = ((TApp)type).mType1;
            IType type2 = ((TApp)type).mType2;
            return mType1.Equals(type1) && mType2.Equals(type2);
        }
        
        public IType Replace(Unique unique, IType type)
        {
            return new TApp(
                mType1.Replace(unique, type),
                mType2.Replace(unique, type)
            );
        }
        
        public string Show()
        {
            if (mType1 is TApp)
            {
                IType type1 = ((TApp)mType1).Function;
                IType type2 = ((TApp)mType1).Argument;
                if (type1 == TCFunc.Instance)
                {
                    return string.Format(
                        "({0} -> {1})", type2.Show(), mType2.Show()
                    );
                }
            }
            return string.Format("{0}({1})", mType1.Show(), mType2.Show());
        }
    }
}
