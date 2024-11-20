using System;
using System.Collections.Generic;
using UnityEngine;
using VMFramework.Core;

namespace VMBehaviorTree
{
    public abstract partial class CompositeNode : ParentOwnerNode
    {
        [HideInInspector]
        [SerializeField]
        protected List<Node> children = new();

        public override void GetChildren(ICollection<Node> nodes)
        {
            nodes.AddRange(children);
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
            
            children.Add(node);
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

            if (children.Remove(node))
            {
                node.RemoveParent(this);
            }
        }

        public override Node Clone(ICollection<Node> clonedNodes)
        {
            var compositeNode = Instantiate(this);
            clonedNodes.Add(compositeNode);

            for (int i = 0; i < children.Count; i++)
            {
                var clonedChild = children[i].Clone(clonedNodes);
                compositeNode.children[i] = clonedChild;
            }
            
            return compositeNode;
        }

        protected override void OnUpdatePerFrame()
        {
            foreach (var child in children)
            {
                child.UpdatePerFrame();
            }
        }

        protected override void OnAbort()
        {
            base.OnAbort();

            foreach (var child in children)
            {
                child.Abort();
            }
        }
    }
}