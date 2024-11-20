using Sirenix.OdinInspector;
using UnityEngine;

namespace VMBehaviorTree
{
    [NodeContents(BTConstants.ACTION_COMMON_NODES, NAME)]
    public sealed partial class WaitNode : ActionNode
    {
        public const string NAME = "Wait";
        
        [SerializeField]
        [MinValue(0)]
        private float duration = 1;
        
        private float startTime;
        
        protected override void OnUpdateStart()
        {
            startTime = Time.time;
        }

        protected override NodeState OnUpdate()
        {
            if (Time.time - startTime >= duration)
            {
                return NodeState.Success;
            }
            
            return NodeState.Running;
        }

        protected override void OnUpdateStop()
        {
            
        }
    }
}