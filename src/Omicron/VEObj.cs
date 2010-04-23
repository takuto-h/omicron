using System.Collections.Generic;
using System.Linq;

namespace Omicron
{
    public class VEObj : IValueExpr
    {
        private IEnumerable<MethodStructureExpr> mMethodStructureExprs;
        
        public VEObj(IEnumerable<MethodStructureExpr> methodStructureExprs)
        {
            mMethodStructureExprs = methodStructureExprs;
        }
        
        public IType Check(
            ITypeCtxt typeCtxt,
            ITypeEnv typeEnv,
            IValueCtxt valueCtxt
        )
        {
            return new TObj(
                mMethodStructureExprs.Select(
                    strExpr => strExpr.Check(typeCtxt, typeEnv, valueCtxt)
                )
            );
        }
        
        public IValue Eval(IValueEnv valueEnv)
        {
            return new VObj(
                mMethodStructureExprs.Select(
                    strExpr => strExpr.Eval(valueEnv)
                ).ToDictionary(str => str.Key, str => str.Value)
            );
        }
        
        public string Show()
        {
            return string.Format(
                "${{{0}}}",
                mMethodStructureExprs.Skip(1).Aggregate(
                    mMethodStructureExprs.ElementAt(0).Show(),
                    (acc, elem) => string.Format("{0}, {1}", acc, elem.Show())
                )
            );
        }
    }
}
