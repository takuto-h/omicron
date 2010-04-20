using System;

namespace Omicron
{
    public class VETypeApp : IValueExpr
    {
        private IValueExpr mValueExpr;
        private ITypeExpr mTypeExpr;
        
        public VETypeApp(IValueExpr valueExpr, ITypeExpr typeExpr)
        {
            mValueExpr = valueExpr;
            mTypeExpr = typeExpr;
        }
        
        public IType Check(
            ITypeCtxt typeCtxt,
            ITypeEnv typeEnv,
            IValueCtxt valueCtxt
        )
        {
            IType type1 = mValueExpr.Check(typeCtxt, typeEnv, valueCtxt);
            IKind kind = mTypeExpr.Check(typeCtxt);
            IType type2 = mTypeExpr.Eval(typeEnv);
            return CheckApp(type1, kind, type2);
        }
        
        public IType CheckApp(IType type1, IKind kind, IType type2)
        {
            /*
            (TApp (TCPoly kind2) (TAbs unique type6)) = type1;
            return CheckPolymorphicTypeApp(kind2, kind, unique, type6, type2);
            */
            if (type1 is TApp)
            {
                IType type4 = ((TApp)type1).Function;
                IType type5 = ((TApp)type1).Argument;
                if (type4 is TCPoly)
                {
                    IKind kind2 = ((TCPoly)type4).Kind;
                    if (type5 is TAbs)
                    {
                        Unique unique = ((TAbs)type5).Parameter;
                        IType type6 = ((TAbs)type5).Body;
                        return CheckPolymorphicTypeApp(
                            kind2, kind, unique, type6, type2
                        );
                    }
                }
            }
            throw new ArgumentException(
                string.Format(
                    "{0} doesn't have a polymorphic ype", mValueExpr.Show()
                )
            );
        }
        
        public IType CheckPolymorphicTypeApp(
            IKind parameterKind,
            IKind argumentKind,
            Unique parameter,
            IType body,
            IType argument
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
            return body.Eval(parameter, argument);
        }
        
        public IValue Eval(IValueEnv valueEnv)
        {
            return mValueExpr.Eval(valueEnv);
        }
        
        public string Show()
        {
            return string.Format(
                "{0}[{1}]", mValueExpr.Show(), mTypeExpr.Show()
            );
        }
    }
}
