using System;
using System.Collections.Generic;
using UnityEngine;

namespace VMBehaviorTree
{
    public sealed partial class RootNode : Node
    {
        [HideInInspector]
        [SerializeField]
        private Node child;

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

        public override void GetChildren(ICollection<Node> nodes)
        {
            nodes.Add(child);
        }

        public override Node GetParent() => null;

        public override void AddParent(Node node)
        {
            throw new InvalidOperationException("Root nodes cannot have parents.");
        }

        public override void RemoveParent(Node node)
        {
            throw new InvalidOperationException("Root nodes cannot have parents.");
        }

        protected override void OnUpdateStart()
        {
            
        }

        protected override NodeState OnUpdate()
        {
            return child.Update();
        }

        protected override void OnUpdateStop()
        {
            
        }

        public override Node Clone(ICollection<Node> clonedNodes)
        {
            var rootNode = Instantiate(this);
            clonedNodes.Add(rootNode);

            if (child != null)
            {
                var childClone = child.Clone(clonedNodes);
                rootNode.child = childClone;
            }
            
            return rootNode;
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