using System.Collections.Generic;

namespace Omicron
{
    public class ValueCtxtLocal : IValueCtxt
    {
        private Dictionary<string, IType> mDictionary;
        private IValueCtxt mParent;
        
        public ValueCtxtLocal(IValueCtxt parent)
        {
            mDictionary = new Dictionary<string, IType>();
            mParent = parent;
        }
        
        public bool TryGetValue(string valueVarName, out IType result)
        {
            return mDictionary.TryGetValue(valueVarName, out result)
              || mParent.TryGetValue(valueVarName, out result);
        }
        
        public void Add(string valueVarName, IType type)
        {
            mDictionary.Add(valueVarName, type);
        }
        
        public IValueCtxt MakeChild()
        {
            return new ValueCtxtLocal(this);
        }
    }
}
