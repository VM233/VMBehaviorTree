using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Sirenix.OdinInspector;
using UnityEngine;

namespace VMBehaviorTree
{
    public sealed partial class Blackboard : SerializedScriptableObject
    {
        [ListDrawerSettings(NumberOfItemsPerPage = 5)]
        public List<IKeyInfo> keyInfos = new();

        [ShowInInspector]
        [HideInEditorMode]
        private readonly Dictionary<string, object> data = new();
        
        public void Initialize()
        {
            foreach (var keyInfo in keyInfos)
            {
                if (data.TryAdd(keyInfo.KeyID, keyInfo.InitialValue))
                {
                    continue;
                }
                
                Debug.LogError($"Duplicate key ID: {keyInfo.KeyID}");
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetValue<TValue>(string keyID, out TValue value)
        {
            if (data.TryGetValue(keyID, out object obj))
            {
                value = (TValue) obj;
                return true;
            }
            
            value = default;
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetValue<TValue>(string keyID, TValue value)
        {
            data[keyID] = value;
        }
    }
}