namespace Omicron
{
    public class VEDef : IValueExpr
    {
        private string mValueVarName;
        private IValueExpr mValueExpr;
        
        public VEDef(string valueVarName, IValueExpr valueExpr)
        {
            mValueVarName = valueVarName;
            mValueExpr = valueExpr;
        }
        
        public IType Check(
            ITypeCtxt typeCtxt,
            ITypeEnv typeEnv,
            IValueCtxt valueCtxt
        )
        {
            IType type = mValueExpr.Check(typeCtxt, typeEnv, valueCtxt);
            valueCtxt.Add(mValueVarName, type);
            return type;
        }
        
        public IValue Eval(IValueEnv valueEnv)
        {
            IValue value = mValueExpr.Eval(valueEnv);
            valueEnv.Add(mValueVarName, value);
            return value;
        }
        
        public string Show()
        {
            return string.Format(
                "def {0} = {1}", mValueVarName, mValueExpr.Show()
            );
        }
    }
}
