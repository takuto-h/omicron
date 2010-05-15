namespace Omicron
{
    public class VCUnit : IValueConst
    {
        private static IValueConst mInstance;
        
        public static IValueConst Instance
        {
            get
            {
                if (mInstance == null)
                {
                    mInstance = new VCUnit();
                }
                return mInstance;
            }
        }
        
        private VCUnit()
        {
        }
        
        public IType ToType()
        {
            return TCUnit.Instance;
        }
        
        public string Show()
        {
            return "unit";
        }
    }
}
