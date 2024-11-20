using System;
using System.Collections.Generic;
using UnityEngine;

namespace VMBehaviorTree
{
    public abstract partial class DecoratorNode : ParentOwnerNode
    {
        [HideInInspector]
        [SerializeField]
        protected Node child;

        public override void GetChildren(ICollection<Node> nodes)
        {
            nodes.Add(child);
        }

        public override void AddChild(Node node)
        {
            if (node == null)
            {
                return;
            }
            
            var nodeParent = node.GetParent();

            if (nodeParent != null)
            {
                throw new InvalidOperationException("Cannot add a child that already has a parent.");
            }
            
            child = node;
            node.AddParent(this);
        }

        public override void RemoveChild(Node node)
        {
            if (node == null)
            {
                return;
            }
            
            var nodeParent = node.GetParent();

            if (nodeParent != this)
            {
                throw new InvalidOperationException("Cannot remove a child that does not belong to this node.");
            }
            
            if (child == node)
            {
                child = null;
                node.RemoveParent(this);
            }
        }

        public override Node Clone(ICollection<Node> clonedNodes)
        {
            var decoratorNode = Instantiate(this);
            clonedNodes.Add(decoratorNode);

            if (child != null)
            {
                var childClone = child.Clone(clonedNodes);
                decoratorNode.child = childClone;
            }
            
            return decoratorNode;
        }

        protected override void OnUpdatePerFrame()
        {
            if (child != null)
            {
                child.UpdatePerFrame();
            }
        }

        protected override void OnAbort()
        {
            base.OnAbort();

            if (child != null)
            {
                child.Abort();
            }
        }
    }
}