using System;

namespace Omicron
{
    public class Unique : IShowable
    {
        private static int mCurrentNumber;
        
        private int mNumber;
        
        public Unique()
        {
            mNumber = mCurrentNumber;
            mCurrentNumber++;
        }
        
        public string Show()
        {
            return string.Format("<{0}>", mNumber);
        }
    }
}
