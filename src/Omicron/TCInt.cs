namespace Omicron
{
    public class TCInt : ITypeConst
    {
        private static ITypeConst mInstance;
        
        public static ITypeConst Instance
        {
            get
            {
                if (mInstance == null)
                {
                    mInstance = new TCInt();
                }
                return mInstance;
            }
        }
        
        private TCInt()
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
        
        public string Show()
        {
            return "Int";
        }
    }
}
