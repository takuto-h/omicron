namespace Omicron
{
    public class TCUnit : ITypeConst
    {
        private static ITypeConst mInstance;
        
        public static ITypeConst Instance
        {
            get
            {
                if (mInstance == null)
                {
                    mInstance = new TCUnit();
                }
                return mInstance;
            }
        }
        
        private TCUnit()
        {
        }
        
        public IType Eval(Unique unique, IType type)
        {
            return this;
        }
        
        public bool Equals(IType type)
        {
            return this == type;
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
            return "Unit";
        }
    }
}
