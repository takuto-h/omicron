using System;

namespace Omicron
{
    public class TEVar : ITypeExpr
    {
        private string mTypeVarName;
        
        public TEVar(string typeVarName)
        {
            mTypeVarName = typeVarName;
        }
        
        public IKind Check(ITypeCtxt typeCtxt)
        {
            IKind result;
            if (!typeCtxt.TryGetValue(mTypeVarName, out result))
            {
                throw new InvalidOperationException(
                    string.Format("unknown type variable: {0}", mTypeVarName)
                );
            }
            return result;
        }
        
        public IType Eval(ITypeEnv typeEnv)
        {
            IType result;
            if (!typeEnv.TryGetValue(mTypeVarName, out result))
            {
                throw new InvalidOperationException(
                    string.Format("unbound type variable: {0}", mTypeVarName)
                );
            }
            return result;
        }
        
        public string Show()
        {
            return mTypeVarName;
        }
    }
}
