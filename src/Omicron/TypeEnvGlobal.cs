using System.Collections.Generic;

namespace Omicron
{
    public class TypeEnvGlobal : Dictionary<string, IType>, ITypeEnv
    {
        public TypeEnvGlobal()
        {
            Add("Int", TCInt.Instance);
            Add("String", TCString.Instance);
            Add("Func", TCFunc.Instance);
        }
        
        public ITypeEnv MakeChild()
        {
            return new TypeEnvLocal(this);
        }
    }
}
