using System.Collections.Generic;
using System.Linq;

namespace Omicron
{
    public class TEObj : ITypeExpr
    {
        private IDictionary<string, ITypeExpr> mMethodTypeExprs;
        
        public TEObj(IDictionary<string, ITypeExpr> methodTypeExprs)
        {
            mMethodTypeExprs = methodTypeExprs;
        }
        
        public IKind Check(ITypeCtxt typeCtxt)
        {
            foreach (ITypeExpr typeExpr in mMethodTypeExprs.Values)
            {
                typeExpr.Check(typeCtxt);
            }
            return KCType.Instance;
        }
        
        public IType Eval(ITypeEnv typeEnv)
        {
            var methodTypes = new Dictionary<string, IType>();
            foreach (KeyValuePair<string, ITypeExpr> kvp in mMethodTypeExprs)
            {
                methodTypes.Add(kvp.Key, kvp.Value.Eval(typeEnv));
            }
            return new TObj(methodTypes);
        }
        
        public string Show()
        {
            return string.Format(
                "${{{0}}}",
                mMethodTypeExprs.Skip(1).Aggregate(
                    ShowMethodTypeExpr(mMethodTypeExprs.ElementAt(0)),
                    (acc, elem) => string.Format(
                        "{0}, {1}", acc, ShowMethodTypeExpr(elem)
                    )
                )
            );
        }
        
        private static string ShowMethodTypeExpr(
            KeyValuePair<string, ITypeExpr> kvp
        )
        {
            return string.Format("{0}:{1}", kvp.Key, kvp.Value.Show());
        }
    }
}
