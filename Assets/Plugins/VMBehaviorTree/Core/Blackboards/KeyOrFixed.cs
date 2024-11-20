using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Sirenix.OdinInspector;
using UnityEngine;

namespace VMBehaviorTree
{
    public partial class KeyOrFixed<TValue> : IKey<TValue>
    {
        private Blackboard blackboard;

        [SerializeField]
        private bool isFixed;
        
        [ShowIf(nameof(isFixed))]
        [SerializeField]
        private TValue fixedValue;
        
#if UNITY_EDITOR
        [ValueDropdown(nameof(GetPossibleKeysID))]
        [HideIf(nameof(isFixed))]
#endif
        [SerializeField]
        private string keyID;

        public KeyOrFixed() : this(default)
        {
            
        }

        public KeyOrFixed(TValue value)
        {
            isFixed = true;
            fixedValue = value;
        }

        void IKey.SetBlackboard(Blackboard blackboard)
        {
            this.blackboard = blackboard;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TValue GetValue()
        {
            if (isFixed)
            {
                return fixedValue;
            }
            
            if (blackboard.TryGetValue(keyID, out TValue value) == false)
            {
                throw new KeyNotFoundException(keyID);
            }
            
            return value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue(TValue value)
        {
            if (isFixed)
            {
                fixedValue = value;
                return;
            }
            
            blackboard.SetValue(keyID, value);
        }

        public override string ToString()
        {
            if (isFixed)
            {
                return fixedValue?.ToString();
            }

            return $"[Key: {keyID}]";
        }
    }
}