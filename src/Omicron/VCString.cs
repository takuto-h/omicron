namespace Omicron
{
    public class VCString : IValueConst
    {
        private string mStringValue;
        
        public VCString(string intValue)
        {
            mStringValue = intValue;
        }
        
        public IType ToType()
        {
            return TCString.Instance;
        }
        
        public string Show()
        {
            return string.Format("\"{0}\"", mStringValue);
        }
    }
}
