using System.Collections.Generic;

namespace Omicron
{
    public class MethodStructureExpr : IShowable
    {
        private string mMethodName;
        private IValueExpr mMethodValueExpr;
        
        public MethodStructureExpr(string methodName, IValueExpr methodValueExpr)
        {
            mMethodName = methodName;
            mMethodValueExpr = methodValueExpr;
        }
        
        public MethodSignature Check(
            ITypeCtxt typeCtxt,
            ITypeEnv typeEnv,
            IValueCtxt valueCtxt
        )
        {
            return new MethodSignature(
                mMethodName,
                mMethodValueExpr.Check(typeCtxt, typeEnv, valueCtxt)
            );
        }
        
        public KeyValuePair<string, IValue> Eval(IValueEnv valueEnv)
        {
            return new KeyValuePair<string, IValue>(
                mMethodName,
                mMethodValueExpr.Eval(valueEnv)
            );
        }
        
        public string Show()
        {
            return string.Format(
                "{0}={1}", mMethodName, mMethodValueExpr.Show()
            );
        }
    }
}
