using System;

namespace Omicron
{
    public class MethodSignature : IShowable, IEquatable<MethodSignature>
    {
        private string mMethodName;
        private IType mMethodType;
        
        public MethodSignature(string methodName, IType methodType)
        {
            mMethodName = methodName;
            mMethodType = methodType;
        }
        
        public MethodSignature Replace(Unique unique, IType type)
        {
            return new MethodSignature(
                mMethodName, mMethodType.Replace(unique, type)
            );
        }
        
        public MethodSignature Eval(Unique unique, IType type)
        {
            return new MethodSignature(
                mMethodName, mMethodType.Eval(unique, type)
            );
        }
        
        public bool Equals(MethodSignature sig)
        {
            return sig.mMethodName == mMethodName
              && sig.mMethodType.Equals(mMethodType);
        }
        
        public string Show()
        {
            return string.Format(
                "{0}:{1}", mMethodName, mMethodType.Show()
            );
        }
    }
}
