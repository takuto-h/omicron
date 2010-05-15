using System.Collections.Generic;

namespace Omicron
{
    public class ValueEnvGlobal : Dictionary<string, IValue>, IValueEnv
    {
        public ValueEnvGlobal()
        {
            Add("unit", VCUnit.Instance);
        }
        
        public IValueEnv MakeChild()
        {
            return new ValueEnvLocal(this);
        }
    }
}
