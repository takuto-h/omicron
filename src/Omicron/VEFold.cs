using System;

namespace Omicron
{
    public class VEFold : IValueExpr
    {
        private ITypeExpr mTypeExpr;
        private IValueExpr mValueExpr;
        
        public VEFold(ITypeExpr typeExpr, IValueExpr valueExpr)
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
            IType unrolledType = Unroll(rolledType);
            IType valueExprType = mValueExpr.Check(
                typeCtxt, typeEnv, valueCtxt
            );
            if (!valueExprType.Equals(unrolledType))
            {
                throw new InvalidOperationException(
                    string.Format(
                        "{0} cannot be folded with the type {1}",
                        mValueExpr.Show(),
                        mTypeExpr.Show()
                    )
                );
            }
            return rolledType;
        }
        
        private IType Unroll(IType type)
        {
            /*
            (TApp TCRec (TAbs unique type3)) = type
            return UnrollRecursiveType(type3, unique, type);
            */
            if (type is TApp)
            {
                IType type1 = ((TApp)type).Function;
                IType type2 = ((TApp)type).Argument;
                if (type1 is TCRec)
                {
                    if (type2 is TAbs)
                    {
                        Unique unique = ((TAbs)type2).Parameter;
                        IType type3 = ((TAbs)type2).Body;
                        return type3.Replace(unique, type);
                    }
                }
            }
            throw new ArgumentException(
                string.Format(
                    "{0} is not a recursive type", mTypeExpr.Show()
                )
            );
        }
        
        public IValue Eval(IValueEnv valueEnv)
        {
            return mValueExpr.Eval(valueEnv);
        }
        
        public string Show()
        {
            return string.Format(
                "fold[{0}]({1})", mTypeExpr.Show(), mValueExpr.Show()
            );
        }
    }
}
