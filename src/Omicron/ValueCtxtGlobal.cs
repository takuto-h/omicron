using System.Collections.Generic;

namespace Omicron
{
    public class ValueCtxtGlobal : Dictionary<string, IType>, IValueCtxt
    {
        public ValueCtxtGlobal()
        {
            Add("unit", TCUnit.Instance);
        }
        
        public IValueCtxt MakeChild()
        {
            return new ValueCtxtLocal(this);
        }
    }
}
