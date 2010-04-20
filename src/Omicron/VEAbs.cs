using System;

namespace Omicron
{
    public class VEAbs : IValueExpr
    {
        private string mValueVarName;
        private ITypeExpr mTypeExpr;
        private IValueExpr mValueExpr;
        
        public VEAbs(
            string valueVarName,
            ITypeExpr typeExpr,
            IValueExpr valueExpr
        )
        {
            mValueVarName = valueVarName;
            mTypeExpr = typeExpr;
            mValueExpr = valueExpr;
        }
        
        public IType Check(
            ITypeCtxt typeCtxt,
            ITypeEnv typeEnv,
            IValueCtxt valueCtxt
        )
        {
            IKind kind = mTypeExpr.Check(typeCtxt);
            if (!(kind is IKindConst))
            {
                throw new InvalidOperationException(
                    string.Format(
                        "{0} doesn't have a base kind", mTypeExpr.Show()
                    )
                );
            }
            IType type1 = mTypeExpr.Eval(typeEnv);
            valueCtxt = valueCtxt.MakeChild();
            valueCtxt.Add(mValueVarName, type1);
            IType type2 = mValueExpr.Check(typeCtxt, typeEnv, valueCtxt);
            return MakeFunctionType(type1, type2);
        }
        
        public IValue Eval(IValueEnv valueEnv)
        {
            return new VClosure(valueEnv, mValueVarName, mValueExpr);
        }
        
        private static IType MakeFunctionType(IType type1, IType type2)
        {
            return new TApp(new TApp(TCFunc.Instance, type1), type2);
        }
        
        public string Show()
        {
            return string.Format(
                "^{0}:{1}{{{2}}}",
                mValueVarName, mTypeExpr.Show(), mValueExpr.Show()
            );
        }
    }
}
