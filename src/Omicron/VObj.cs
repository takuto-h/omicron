using System.Collections.Generic;
using System.Linq;

namespace Omicron
{
    public class VObj : IValue
    {
        private IDictionary<string, IValue> mMethodTable;
        
        public VObj(IDictionary<string, IValue> methodTable)
        {
            mMethodTable = methodTable;
        }
        
        public string Show()
        {
            IEnumerable<string> methodNames = mMethodTable.Keys;
            return string.Format(
                "${{{0}}}",
                methodNames.Skip(1).Aggregate(
                    methodNames.ElementAt(0),
                    (acc, elem) => string.Format("{0}, {1}", acc, elem)
                )
            );
        }
    }
}
