using System.Collections.Generic;

namespace Omicron
{
    public class TypeEnvGlobal : Dictionary<string, IType>, ITypeEnv
    {
        public TypeEnvGlobal()
        {
            Add("Unit", TCUnit.Instance);
            Add("Int", TCInt.Instance);
            Add("String", TCString.Instance);
            Add("Func", TCFunc.Instance);
            Add("Rec", TCRec.Instance);
        }
        
        public ITypeEnv MakeChild()
        {
            return new TypeEnvLocal(this);
        }
    }
}
