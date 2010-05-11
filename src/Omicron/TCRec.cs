namespace Omicron
{
    public class TCRec : ITypeConst
    {
        private static ITypeConst mInstance;
        
        public static ITypeConst Instance
        {
            get
            {
                if (mInstance == null)
                {
                    mInstance = new TCRec();
                }
                return mInstance;
            }
        }
        
        private TCRec()
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
            return "Rec";
        }
    }
}
