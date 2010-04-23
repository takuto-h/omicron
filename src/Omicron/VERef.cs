using System;

namespace Omicron
{
    public class VERef : IValueExpr
    {
        private IValueExpr mValueExpr;
        private string mMethodName;
        
        public VERef(IValueExpr valueExpr, string methodName)
        {
            mValueExpr = valueExpr;
            mMethodName = methodName;
        }
        
        public IType Check(
            ITypeCtxt typeCtxt,
            ITypeEnv typeEnv,
            IValueCtxt valueCtxt
        )
        {
            IType type = mValueExpr.Check(typeCtxt, typeEnv, valueCtxt);
            if (!(type is TObj))
            {
                throw new InvalidOperationException(
                    string.Format(
                        "{0} doesn't have an object type", mValueExpr.Show()
                    )
                );
            }
            return ((TObj)type).GetMethodType(mMethodName);
        }
        
        public IValue Eval(IValueEnv valueEnv)
        {
            IValue value = mValueExpr.Eval(valueEnv);
            if (!(value is VObj))
            {
                throw new InvalidOperationException(
                    string.Format("object required, but got {0}", value.Show())
                );
            }
            return ((VObj)value).GetMethodValue(mMethodName);
        }
        
        public string Show()
        {
            return string.Format("{0}.{1}", mValueExpr.Show(), mMethodName);
        }
    }
}
