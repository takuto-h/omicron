using System;

namespace Omicron
{
    public class VEUnfold : IValueExpr
    {
        private ITypeExpr mTypeExpr;
        private IValueExpr mValueExpr;
        
        public VEUnfold(ITypeExpr typeExpr, IValueExpr valueExpr)
        {
            mTypeExpr = typeExpr;
            mValueExpr = valueExpr;
        }
        
        public IType Check(
            ITypeCtxt typeCtxt,
            ITypeEnv typeEnv,
            IValueCtxt valueCtxt
        )
        {
            mTypeExpr.Check(typeCtxt);
            IType rolledType = mTypeExpr.Eval(typeEnv);
            IType unrolledType = VEFold.Unroll(rolledType);
            IType valueExprType = mValueExpr.Check(
                typeCtxt, typeEnv, valueCtxt
            );
            if (!valueExprType.Equals(rolledType))
            {
                throw new InvalidOperationException(
                    string.Format(
                        "{0} cannot be unfolded with the type {1}",
                        mValueExpr.Show(),
                        mTypeExpr.Show()
                    )
                );
            }
            return unrolledType;
        }
        
        public IValue Eval(IValueEnv valueEnv)
        {
            return mValueExpr.Eval(valueEnv);
        }
        
        public string Show()
        {
            return string.Format(
                "unfold[{0}]({1})", mTypeExpr.Show(), mValueExpr.Show()
            );
        }
    }
}
