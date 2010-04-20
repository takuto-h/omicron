using System.Collections.Generic;

namespace Omicron
{
    public class TypeCtxtGlobal : Dictionary<string, IKind>, ITypeCtxt
    {
        public TypeCtxtGlobal()
        {
            Add("Int", KCType.Instance);
            Add("String", KCType.Instance);
            Add(
                "Func",
                new KArrow(
                    KCType.Instance,
                    new KArrow(KCType.Instance, KCType.Instance)
                )
            );
        }
        
        public ITypeCtxt MakeChild()
        {
            return new TypeCtxtLocal(this);
        }
    }
}
