using System;

namespace Omicron
{
    public class MethodSignatureExpr : IShowable
    {
        private string mMethodName;
        private ITypeExpr mMethodTypeExpr;
        
        public MethodSignatureExpr(string methodName, ITypeExpr methodTypeExpr)
        {
            mMethodName = methodName;
            mMethodTypeExpr = methodTypeExpr;
        }
        
        public void Check(ITypeCtxt typeEnv)
        {
            IKind kind = mMethodTypeExpr.Check(typeEnv);
            if (!(kind is IKindConst))
            {
                throw new InvalidOperationException(
                    string.Format(
                        "{0} doesn't have a base kind", mMethodTypeExpr.Show()
                    )
                );
            }
        }
        
        public MethodSignature Eval(ITypeEnv typeEnv)
        {
            return new MethodSignature(
                mMethodName, mMethodTypeExpr.Eval(typeEnv)
            );
        }
        
        public string Show()
        {
            return string.Format(
                "{0}:{1}", mMethodName, mMethodTypeExpr.Show()
            );
        }
    }
}
