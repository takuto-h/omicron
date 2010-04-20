using System.Collections.Generic;

namespace Omicron
{
    public class TypeEnvLocal : ITypeEnv
    {
        private Dictionary<string, IType> mDictionary;
        private ITypeEnv mParent;
        
        public TypeEnvLocal(ITypeEnv parent)
        {
            mDictionary = new Dictionary<string, IType>();
            mParent = parent;
        }
        
        public bool TryGetValue(string typeVarName, out IType result)
        {
            return mDictionary.TryGetValue(typeVarName, out result)
              || mParent.TryGetValue(typeVarName, out result);
        }
        
        public void Add(string typeVarName, IType type)
        {
            mDictionary.Add(typeVarName, type);
        }
        
        public ITypeEnv MakeChild()
        {
            return new TypeEnvLocal(this);
        }
    }
}
