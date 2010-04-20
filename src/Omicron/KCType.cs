namespace Omicron
{
    public class KCType : IKindConst
    {
        private static IKindConst mInstance;
        
        public static IKindConst Instance
        {
            get
            {
                if (mInstance == null)
                {
                    mInstance = new KCType();
                }
                return mInstance;
            }
        }
        
        private KCType()
        {
        }
        
        public bool Equals(IKind kind)
        {
            return this == kind;
        }
        
        public string Show()
        {
            return "*";
        }
    }
}
