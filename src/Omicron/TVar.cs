namespace Omicron
{
    public class TVar : IType
    {
        private Unique mUnique;
        
        public TVar(Unique unique)
        {
            mUnique = unique;
        }
        
        public IType Eval(Unique unique, IType type)
        {
            if (mUnique == unique)
            {
                return type;
            }
            else
            {
                return this;
            }
        }
        
        public bool Equals(IType type)
        {
            if (!(type is TVar))
            {
                return false;
            }
            return mUnique == ((TVar)type).mUnique;
        }
        
        public IType Replace(Unique unique, IType type)
        {
            if (mUnique == unique)
            {
                return type;
            }
            else
            {
                return this;
            }
        }
        
        public string Show()
        {
            return mUnique.Show();
        }
    }
}
