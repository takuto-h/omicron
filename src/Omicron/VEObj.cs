using System.Collections.Generic;
using System.Linq;

namespace Omicron
{
    public class VEObj : IValueExpr
    {
        private IDictionary<string, IValueExpr> mMethodValueExprs;
        
        public VEObj(IDictionary<string, IValueExpr> methodValueExprs)
        {
            mMethodValueExprs = methodValueExprs;
        }
        
        public IType Check(
            ITypeCtxt typeCtxt,
            ITypeEnv typeEnv,
            IValueCtxt valueCtxt
        )
        {
            var methodTypes = new Dictionary<string, IType>();
            foreach (KeyValuePair<string, IValueExpr> kvp in mMethodValueExprs)
            {
                methodTypes.Add(
                    kvp.Key, kvp.Value.Check(typeCtxt, typeEnv, valueCtxt)
                );
            }
            return new TObj(methodTypes);
        }
        
        public IValue Eval(IValueEnv valueEnv)
        {
            var methodValues = new Dictionary<string, IValue>();
            foreach (KeyValuePair<string, IValueExpr> kvp in mMethodValueExprs)
            {
                methodValues.Add(kvp.Key, kvp.Value.Eval(valueEnv));
            }
            return new VObj(methodValues);
        }
        
        public string Show()
        {
            if (mMethodValueExprs.Count == 0)
            {
                return "${}";
            }
            return string.Format(
                "${{{0}}}",
                mMethodValueExprs.Skip(1).Aggregate(
                    ShowMethodValueExpr(mMethodValueExprs.ElementAt(0)),
                    (acc, elem) => string.Format(
                        "{0}, {1}", acc, ShowMethodValueExpr(elem)
                    )
                )
            );
        }
        
        private static string ShowMethodValueExpr(
            KeyValuePair<string, IValueExpr> kvp
        )
        {
            return string.Format("{0}={1}", kvp.Key, kvp.Value.Show());
        }
    }
}
