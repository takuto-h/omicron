namespace Omicron
{
    public class TCString : ITypeConst
    {
        private static ITypeConst mInstance;
        
        public static ITypeConst Instance
        {
            get
            {
                if (mInstance == null)
                {
                    mInstance = new TCString();
                }
                return mInstance;
            }
        }
        
        private TCString()
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
            return "String";
        }
    }
}
