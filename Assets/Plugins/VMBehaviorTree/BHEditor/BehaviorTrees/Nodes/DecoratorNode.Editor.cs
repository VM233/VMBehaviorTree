#if UNITY_EDITOR
using System;

namespace VMBehaviorTree
{
    public partial class DecoratorNode
    {
        public override bool HasInput => true;

        public override NodePortType OutputPortType => NodePortType.Single;
    }
}
#endif