using System;
using System.Collections.Generic;
using System.Linq;

namespace Omicron
{
    public class TObj : IType
    {
        private IDictionary<string, IType> mMethodTypes;
        
        public TObj(IDictionary<string, IType> methodTypes)
        {
            mMethodTypes = methodTypes;
        }
        
        public IType Replace(Unique unique, IType type)
        {
            var methodTypes = new Dictionary<string, IType>();
            foreach (KeyValuePair<string, IType> kvp in mMethodTypes)
            {
                methodTypes.Add(kvp.Key, kvp.Value.Replace(unique, type));
            }
            return new TObj(methodTypes);
        }
        
        public IType Eval(Unique unique, IType type)
        {
            var methodTypes = new Dictionary<string, IType>();
            foreach (KeyValuePair<string, IType> kvp in mMethodTypes)
            {
                methodTypes.Add(kvp.Key, kvp.Value.Eval(unique, type));
            }
            return new TObj(methodTypes);
        }
        
        public IType GetMethodType(string methodName)
        {
            IType result;
            if (!mMethodTypes.TryGetValue(methodName, out result))
            {
                throw new InvalidOperationException(
                    string.Format(
                        "undefined method for {0}: {1}", Show(), methodName
                    )
                );
            }
            return result;
        }
        
        public bool Equals(IType type)
        {
            if (!(type is TObj))
            {
                return false;
            }
            var methodTypes = new Dictionary<string, IType>(
                ((TObj)type).mMethodTypes
            );
            foreach (KeyValuePair<string, IType> kvp in mMethodTypes)
            {
                IType result;
                if (!methodTypes.TryGetValue(kvp.Key, out result))
                {
                    return false;
                }
                else if (!result.Equals(kvp.Value))
                {
                    return false;
                }
                methodTypes.Remove(kvp.Key);
            }
            if (methodTypes.Count != 0)
            {
                return false;
            }
            return true;
        }
        
        public string Show()
        {
            if (mMethodTypes.Count == 0)
            {
                return "${}";
            }
            return string.Format(
                "${{{0}}}",
                mMethodTypes.Skip(1).Aggregate(
                    ShowMethodType(mMethodTypes.ElementAt(0)),
                    (acc, elem) => string.Format(
                        "{0}, {1}", acc, ShowMethodType(elem)
                    )
                )
            );
        }
        
        private static string ShowMethodType(KeyValuePair<string, IType> kvp)
        {
            return string.Format("{0}:{1}", kvp.Key, kvp.Value.Show());
        }
    }
}
