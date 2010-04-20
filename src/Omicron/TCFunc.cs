namespace Omicron
{
    public class TCFunc : ITypeConst
    {
        private static ITypeConst mInstance;
        
        public static ITypeConst Instance
        {
            get
            {
                if (mInstance == null)
                {
                    mInstance = new TCFunc();
                }
                return mInstance;
            }
        }
        
        private TCFunc()
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
            return "Func";
        }
    }
}
