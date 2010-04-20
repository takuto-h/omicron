using System.Collections.Generic;
using System.Linq;

namespace Omicron
{
    public class TEObj : ITypeExpr
    {
        private IEnumerable<MethodSignatureExpr> mMethodSignatureExprs;
        
        public TEObj(IEnumerable<MethodSignatureExpr> methodSignatureExprs)
        {
            mMethodSignatureExprs = methodSignatureExprs;
        }
        
        public IKind Check(ITypeCtxt typeCtxt)
        {
            foreach (MethodSignatureExpr sigExpr in mMethodSignatureExprs)
            {
                sigExpr.Check(typeCtxt);
            }
            return KCType.Instance;
        }
        
        public IType Eval(ITypeEnv typeEnv)
        {
            return new TObj(
                mMethodSignatureExprs.Select(sigExpr => sigExpr.Eval(typeEnv))
            );
        }
        
        public string Show()
        {
            
            return string.Format(
                "${{{0}}}",
                mMethodSignatureExprs.Aggregate(
                    "",
                    (acc, elem) => string.Format("{0}, {1}", acc, elem.Show())
                )
            );
        }
    }
}
