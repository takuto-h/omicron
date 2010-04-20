using System.Collections.Generic;
using System.Linq;

namespace Omicron
{
    public class TObj : IType
    {
        private IEnumerable<MethodSignature> mMethodSignatures;
        
        public TObj(IEnumerable<MethodSignature> methodSignatures)
        {
            mMethodSignatures = methodSignatures;
        }
        
        public IType Replace(Unique unique, IType type)
        {
            return new TObj(
                mMethodSignatures.Select(sig => sig.Replace(unique, type))
            );
        }
        
        public IType Eval(Unique unique, IType type)
        {
            return new TObj(
                mMethodSignatures.Select(sig => sig.Eval(unique, type))
            );
        }
        
        public bool Equals(IType type)
        {
            if (!(type is TObj))
            {
                return false;
            }
            return ((TObj)type).mMethodSignatures
              .Except(mMethodSignatures).Count() == 0;
        }
        
        public string Show()
        {
            return string.Format(
                "${{{0}}}",
                mMethodSignatures.Skip(1).Aggregate(
                    mMethodSignatures.ElementAt(0).Show(),
                    (acc, elem) => string.Format("{0}, {1}", acc, elem.Show())
                )
            );
        }
    }
}
