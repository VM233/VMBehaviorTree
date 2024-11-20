#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;

namespace VMBehaviorTree
{
    [HideReferenceObjectPicker]
    public partial class KeyOrFixed<TValue>
    {
        private IEnumerable<string> GetPossibleKeysID()
        {
            if (blackboard == null)
            {
                return Enumerable.Empty<string>();
            }

            return blackboard.GetKeysOfType(typeof(TValue));
        }
    }
}
#endif