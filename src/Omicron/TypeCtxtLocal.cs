using System.Collections.Generic;

namespace Omicron
{
    public class TypeCtxtLocal : ITypeCtxt
    {
        private Dictionary<string, IKind> mDictionary;
        private ITypeCtxt mParent;
        
        public TypeCtxtLocal(ITypeCtxt parent)
        {
            mDictionary = new Dictionary<string, IKind>();
            mParent = parent;
        }
        
        public bool TryGetValue(string valueVarName, out IKind result)
        {
            return mDictionary.TryGetValue(valueVarName, out result)
              || mParent.TryGetValue(valueVarName, out result);
        }
        
        public void Add(string valueVarName, IKind kind)
        {
            mDictionary.Add(valueVarName, kind);
        }
        
        public ITypeCtxt MakeChild()
        {
            return new TypeCtxtLocal(this);
        }
    }
}
