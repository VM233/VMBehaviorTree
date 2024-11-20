#if UNITY_EDITOR
using Sirenix.OdinInspector;
using VMBehaviorTree.Editor;

namespace VMBehaviorTree
{
    public partial class BehaviorTreeRunner
    {
        [Button]
        private void OpenInEditor()
        {
            BehaviorTreeEditor.Open(this);
        }
    }
}
#endif