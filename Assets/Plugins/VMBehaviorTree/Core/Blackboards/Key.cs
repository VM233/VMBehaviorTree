using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Sirenix.OdinInspector;
using UnityEngine;

namespace VMBehaviorTree
{
    public partial class Key<TValue> : IKey<TValue>
    {
        private Blackboard blackboard;

#if UNITY_EDITOR
        [ValueDropdown(nameof(GetPossibleKeysID))]
#endif
        [SerializeField]
        private string keyID;

        void IKey.SetBlackboard(Blackboard blackboard)
        {
            this.blackboard = blackboard;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TValue GetValue()
        {
            if (blackboard.TryGetValue(keyID, out TValue value) == false)
            {
                throw new KeyNotFoundException(keyID);
            }
            
            return value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(TValue value)
        {
            blackboard.SetValue(keyID, value);
        }

        public override string ToString()
        {
            return $"[Key: {keyID}]";
        }
    }
}