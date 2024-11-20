#if UNITY_EDITOR
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using VMBehaviorTree.Editor;

namespace VMBehaviorTree
{
    public partial class BehaviorTree
    {
        [HideInInspector]
        public Vector3 viewPosition;
        [HideInInspector]
        public Vector3 viewScale;
        
        [OnOpenAsset]
        private static bool OnOpenAsset(int instanceID, int line)
        {
            var obj = EditorUtility.InstanceIDToObject(instanceID);
            if (obj is BehaviorTree behaviorTree)
            {
                BehaviorTreeEditor.Open(behaviorTree);
                return true;
            }
            
            return false;
        }
        
        [Button]
        private void OpenInEditor()
        {
            BehaviorTreeEditor.Open(this);
        }
    }
}
#endif