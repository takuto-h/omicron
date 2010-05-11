namespace Omicron
{
    public class TAbs : ITypeFunc
    {
        private Unique mUnique;
        private IType mType;
        
        public Unique Parameter
        {
            get { return mUnique; }
        }
        
        public IType Body
        {
            get { return mType; }
        }
        
        public TAbs(Unique unique, IType type)
        {
            mUnique = unique;
            mType = type;
        }
        
        public IType Eval(Unique unique, IType type)
        {
            return new TAbs(mUnique, mType.Eval(unique, type));
        }
        
        public IType Apply(IType type)
        {
            return mType.Eval(mUnique, type);
        }
        
        public bool Equals(IType type)
        {
            if (!(type is TAbs))
            {
                return false;
            }
            Unique unique2 = ((TAbs)type).mUnique;
            IType type2 = ((TAbs)type).mType;
            return mType.Replace(mUnique, new TVar(unique2)).Equals(type2);
        }
        
        public IType Replace(Unique unique, IType type)
        {
            return new TAbs(mUnique, mType.Replace(unique, type));
        }
        
        public IType Rename()
        {
            Unique newUnique = new Unique();
            return new TAbs(
                newUnique,
                mType.Replace(mUnique, new TVar(newUnique)).Rename()
            );
        }
        
        public string Show()
        {
            return string.Format(
                "^{0}{{{1}}}", mUnique.Show(), mType.Show()
            );
        }
    }
}
