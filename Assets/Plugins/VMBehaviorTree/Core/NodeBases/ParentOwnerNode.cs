using UnityEngine;

namespace VMBehaviorTree
{
    public abstract class ParentOwnerNode : Node
    {
        [HideInInspector]
        [SerializeField]
        private Node parent;

        public override Node GetParent() => parent;

        public override void AddParent(Node node)
        {
            
            if (node == null)
            {
                return;
            }
            
            parent = node;
        }

        public override void RemoveParent(Node node)
        {
            if (node == null)
            {
                return;
            }
            
            if (parent == node)
            {
                parent = null;
            }
        }
    }
}