using System.Collections.Generic;

namespace Omicron
{
    public class ValueEnvLocal : IValueEnv
    {
        private Dictionary<string, IValue> mDictionary;
        private IValueEnv mParent;
        
        public ValueEnvLocal(IValueEnv parent)
        {
            mDictionary = new Dictionary<string, IValue>();
            mParent = parent;
        }
        
        public bool TryGetValue(string valueVarName, out IValue result)
        {
            return mDictionary.TryGetValue(valueVarName, out result)
              || mParent.TryGetValue(valueVarName, out result);
        }
        
        public void Add(string valueVarName, IValue value)
        {
            mDictionary.Add(valueVarName, value);
        }
        
        public IValueEnv MakeChild()
        {
            return new ValueEnvLocal(this);
        }
    }
}
