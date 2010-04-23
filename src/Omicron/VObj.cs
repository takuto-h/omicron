using System;
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
        
        public IValue GetMethodValue(string methodName)
        {
            IValue result;
            if (!mMethodValues.TryGetValue(methodName, out result))
            {
                throw new InvalidOperationException(
                    string.Format(
                        "undefined method for {0}: {1}", Show(), methodName
                    )
                );
            }
            return result;
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
