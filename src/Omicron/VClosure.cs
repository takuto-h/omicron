namespace Omicron
{
    public class VClosure : IValueFunc
    {
        private IValueEnv mValueEnv;
        private string mValueVarName;
        private IValueExpr mValueExpr;
        
        public VClosure(
            IValueEnv valueEnv,
            string valueVarName,
            IValueExpr valueExpr
        )
        {
            mValueEnv = valueEnv;
            mValueVarName = valueVarName;
            mValueExpr = valueExpr;
        }
        
        public IValue Apply(IValue value)
        {
            IValueEnv valueEnv = mValueEnv.MakeChild();
            valueEnv.Add(mValueVarName, value);
            return mValueExpr.Eval(valueEnv);
        }
        
        public string Show()
        {
            return string.Format("#<closure 0x{0:x8}>", GetHashCode());
        }
    }
}
