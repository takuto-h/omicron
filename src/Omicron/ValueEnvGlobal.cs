using System.Collections.Generic;

namespace Omicron
{
    public class ValueEnvGlobal : Dictionary<string, IValue>, IValueEnv
    {
        public ValueEnvGlobal()
        {
        }
        
        public IValueEnv MakeChild()
        {
            return new ValueEnvLocal(this);
        }
    }
}
