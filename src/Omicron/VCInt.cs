namespace Omicron
{
    public class VCInt : IValueConst
    {
        private int mIntValue;
        
        public VCInt(int intValue)
        {
            mIntValue = intValue;
        }
        
        public IType ToType()
        {
            return TCInt.Instance;
        }
        
        public string Show()
        {
            return mIntValue.ToString();
        }
    }
}
