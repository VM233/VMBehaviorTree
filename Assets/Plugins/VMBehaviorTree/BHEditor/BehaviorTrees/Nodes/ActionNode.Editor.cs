#if UNITY_EDITOR
using System;

namespace VMBehaviorTree
{
    public partial class ActionNode
    {
        public override bool HasInput => true;

        public override NodePortType OutputPortType => NodePortType.None;
    }
}
#endif