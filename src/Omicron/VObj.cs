using System.Collections.Generic;
using System.Linq;

namespace Omicron
{
    public class VObj : IValue
    {
        private IDictionary<string, IValue> mMethodValues;
        
        public VObj(IDictionary<string, IValue> methodValues)
        {
            mMethodValues = methodValues;
        }
        
        public string Show()
        {
            return string.Format(
                "${{{0}}}",
                mMethodValues.Skip(1).Aggregate(
                    ShowMethodValue(mMethodValues.ElementAt(0)),
                    (acc, elem) => string.Format(
                        "{0}, {1}", acc, ShowMethodValue(elem)
                    )
                )
            );
        }
        
        private static string ShowMethodValue(KeyValuePair<string, IValue> kvp)
        {
            return string.Format("{0}={1}", kvp.Key, kvp.Value.Show());
        }
    }
}
