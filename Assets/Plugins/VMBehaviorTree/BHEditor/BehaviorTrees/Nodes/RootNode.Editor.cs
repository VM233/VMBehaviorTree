#if UNITY_EDITOR
using System;

namespace VMBehaviorTree
{
    public partial class RootNode
    {
        public override bool HasInput => false;

        public override NodePortType OutputPortType => NodePortType.Single;
    }
}
#endif