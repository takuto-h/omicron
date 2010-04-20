namespace Omicron
{
    public class VEConst : IValueExpr
    {
        private IValueConst mValueConst;
        
        public VEConst(IValueConst valueConst)
        {
            mValueConst = valueConst;
        }
        
        public IType Check(
            ITypeCtxt typeCtxt,
            ITypeEnv typeEnv,
            IValueCtxt valueCtxt
        )
        {
            return mValueConst.ToType();
        }
        
        public IValue Eval(IValueEnv valueEnv)
        {
            return mValueConst;
        }
        
        public string Show()
        {
            return mValueConst.Show();
        }
    }
}
