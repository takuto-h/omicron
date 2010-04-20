using System;

namespace Omicron
{
    public class VEApp : IValueExpr
    {
        private IValueExpr mValueExpr1;
        private IValueExpr mValueExpr2;
        
        public VEApp(IValueExpr valueExpr1, IValueExpr valueExpr2)
        {
            mValueExpr1 = valueExpr1;
            mValueExpr2 = valueExpr2;
        }
        
        public IType Check(
            ITypeCtxt typeCtxt,
            ITypeEnv typeEnv,
            IValueCtxt valueCtxt
        )
        {
            IType type1 = mValueExpr1.Check(typeCtxt, typeEnv, valueCtxt);
            IType type2 = mValueExpr2.Check(typeCtxt, typeEnv, valueCtxt);
            return CheckApp(type1, type2);
        }
        
        private IType CheckApp(IType type1, IType type2)
        {
            /*
            (TApp (TApp TCFunc type6) type4) = type1;
            return CheckFunctionTypeApp(type6, type4, type2);
             */
            if (type1 is TApp)
            {
                IType type3 = ((TApp)type1).Function;
                IType type4 = ((TApp)type1).Argument;
                if (type3 is TApp)
                {
                    IType type5 = ((TApp)type3).Function;
                    IType type6 = ((TApp)type3).Argument;
                    if (type5 == TCFunc.Instance)
                    {
                        return CheckFunctionTypeApp(type6, type4, type2);
                    }
                }
            }
            throw new ArgumentException(
                string.Format(
                    "{0} doesn't have a function type", mValueExpr1.Show()
                )
            );
        }
        
        private IType CheckFunctionTypeApp(
            IType parameterType,
            IType returnType,
            IType argumentType
        )
        {
            if (!argumentType.Equals(parameterType))
            {
                throw new ArgumentException(
                    string.Format(
                        "unexpected {0}, expected {1}",
                        argumentType.Show(),
                        parameterType.Show()
                    )
                );
            }
            return returnType;
        }
        
        public IValue Eval(IValueEnv valueEnv)
        {
            IValue value1 = mValueExpr1.Eval(valueEnv);
            IValue value2 = mValueExpr2.Eval(valueEnv);
            IValueFunc func = value1 as IValueFunc;
            if (func == null)
            {
                throw new InvalidOperationException(
                    string.Format("function required, but got {1}", value1)
                );
            }
            return func.Apply(value2);
        }
        
        public string Show()
        {
            return string.Format(
                "{0}({1})", mValueExpr1.Show(), mValueExpr2.Show()
            );
        }
    }
}
