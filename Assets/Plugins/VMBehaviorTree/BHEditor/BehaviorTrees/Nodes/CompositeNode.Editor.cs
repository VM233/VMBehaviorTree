#if UNITY_EDITOR
using System;
using System.Collections.Generic;

namespace VMBehaviorTree
{
    public partial class CompositeNode
    {
        public override bool HasInput => true;

        public override NodePortType OutputPortType => NodePortType.Multiple;

        public override void SortChildren(Comparison<Node> comparison)
        {
            base.SortChildren(comparison);
            
            children.Sort(comparison);
        }
    }
}
#endif