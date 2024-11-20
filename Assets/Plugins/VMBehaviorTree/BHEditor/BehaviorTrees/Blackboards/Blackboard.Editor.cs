#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using VMFramework.Core.Editor;

namespace VMBehaviorTree
{
    public partial class Blackboard
    {
        [PropertyOrder(-100)]
        [HideInPlayMode]
        [Button(Expanded = true)]
        private void AddKey(InstanceTypeInfo valueTypeInfo)
        {
            var keyInfoType = typeof(KeyInfo<>).MakeGenericType(valueTypeInfo.GetInstanceType());
            var keyInfo = (IKeyInfo)Activator.CreateInstance(keyInfoType);
            keyInfos.Add(keyInfo);
            this.SetEditorDirty();
        }
        
        public IEnumerable<string> GetKeysOfType(Type type)
        {
            foreach (var keyInfo in keyInfos)
            {
                if (keyInfo.Type == type)
                {
                    yield return keyInfo.KeyID;
                }
            }
        }
    }
}
#endif