using System.Collections.Generic;

namespace Omicron
{
    public class ValueCtxtGlobal : Dictionary<string, IType>, IValueCtxt
    {
        public ValueCtxtGlobal()
        {
        }
        
        public IValueCtxt MakeChild()
        {
            return new ValueCtxtLocal(this);
        }
    }
}
