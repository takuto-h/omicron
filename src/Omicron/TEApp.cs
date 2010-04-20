using System;

namespace Omicron
{
    public class TEApp : ITypeExpr
    {
        private ITypeExpr mTypeExpr1;
        private ITypeExpr mTypeExpr2;
        
        public TEApp(ITypeExpr typeExpr1, ITypeExpr typeExpr2)
        {
            mTypeExpr1 = typeExpr1;
            mTypeExpr2 = typeExpr2;
        }
        
        public IKind Check(ITypeCtxt typeCtxt)
        {
            IKind kind1 = mTypeExpr1.Check(typeCtxt);
            IKind kind2 = mTypeExpr2.Check(typeCtxt);
            return CheckApp(kind1, kind2);
        }
        
        private IKind CheckApp(IKind kind1, IKind kind2)
        {
            /*
            (KArrow kind3 kind4) = kind1;
            return CheckFuncKindApp(kind3, kind4, kind2);
            */
            if (kind1 is KArrow)
            {
                IKind kind3 = ((KArrow)kind1).Left;
                IKind kind4 = ((KArrow)kind1).Right;
                return CheckFuncKindApp(kind3, kind4, kind2);
            }
            throw new InvalidOperationException(
                string.Format(
                    "{0} doesn't have a function kind", mTypeExpr1.Show()
                )
            );
        }
        
        private IKind CheckFuncKindApp(
            IKind parameterKind,
            IKind returnKind,
            IKind argumentKind
        )
        {
            if (!argumentKind.Equals(parameterKind))
            {
                throw new ArgumentException(
                    string.Format(
                        "unexpected {0}, expected {1}",
                        argumentKind.Show(),
                        parameterKind.Show()
                    )
                );
            }
            return returnKind;
        }
        
        public IType Eval(ITypeEnv typeEnv)
        {
            IType type1 = mTypeExpr1.Eval(typeEnv);
            IType type2 = mTypeExpr2.Eval(typeEnv);
            if (type1 is ITypeFunc)
            {
                return ((ITypeFunc)type1).Apply(type2);
            }
            else
            {
                return new TApp(type1, type2);
            }
        }
        
        public string Show()
        {
            return string.Format(
                "{0}({1})", mTypeExpr1.Show(), mTypeExpr2.Show()
            );
        }
    }
}
