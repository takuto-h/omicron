using System;

namespace Omicron
{
    public class VEVar : IValueExpr
    {
        private string mValueVarName;
        
        public VEVar(string valueVarName)
        {
            mValueVarName = valueVarName;
        }
        
        public IType Check(
            ITypeCtxt typeCtxt,
            ITypeEnv typeEnv,
            IValueCtxt valueCtxt
        )
        {
            IType result;
            if (!valueCtxt.TryGetValue(mValueVarName, out result))
            {
                throw new InvalidOperationException(
                    string.Format("unknown variable: {0}", mValueVarName)
                );
            }
            return result;
        }
        
        public IValue Eval(IValueEnv valueEnv)
        {
            IValue result;
            if (!valueEnv.TryGetValue(mValueVarName, out result))
            {
                throw new InvalidOperationException(
                    string.Format("unbound variable: {0}", mValueVarName)
                );
            }
            return result;
        }
        
        public string Show()
        {
            return mValueVarName;
        }
    }
}
