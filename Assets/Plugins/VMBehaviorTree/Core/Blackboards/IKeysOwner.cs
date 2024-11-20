using System.Collections.Generic;

namespace VMBehaviorTree
{
    public interface IKeysOwner
    {
        public void GetKeys(ICollection<IKey> keys);
    }
}