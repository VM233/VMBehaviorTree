using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using VMBehaviorTree.Interfaces;

namespace VMBehaviorTree
{
    public abstract partial class Node : SerializedScriptableObject, IAbortable
    {
        private NodeState _state;

        public NodeState State
        {
            get => _state;
            private set
            {
                var oldState = _state;
                _state = value;

                if (oldState != value)
                {
                    OnStateChanged?.Invoke(oldState, value);
                }
            }
        }
        
        public event Action<NodeState, NodeState> OnStateChanged;
        
        public bool IsStarted { get; private set; }
        
        [ShowInInspector, ReadOnly]
        public bool HasStateUpdatedThisFrame { get; private set; }

        public void Initialize()
        {
            OnInitialize();
        }

        protected virtual void OnInitialize()
        {
            State = NodeState.Running;
        }

        public NodeState Update()
        {
            HasStateUpdatedThisFrame = true;
            
            if (IsStarted == false)
            {
                OnUpdateStart();
                IsStarted = true;
            }
            
            State = OnUpdate();

            if (State is NodeState.Success or NodeState.Failure)
            {
                OnUpdateStop();
                IsStarted = false;
            }
            
            return State;
        }

        protected abstract void OnUpdateStart();

        protected abstract NodeState OnUpdate();

        protected abstract void OnUpdateStop();

        public void UpdatePerFrame()
        {
            HasStateUpdatedThisFrame = false;
            OnUpdatePerFrame();
        }

        protected abstract void OnUpdatePerFrame();

        public void Stop()
        {
            OnStop();
        }

        protected virtual void OnStop()
        {
            
        }

        public abstract void GetChildren(ICollection<Node> nodes);
        
        public abstract void AddChild(Node node);
        
        public abstract void RemoveChild(Node node);
        
        public abstract Node GetParent();

        public abstract void AddParent(Node node);

        public abstract void RemoveParent(Node node);

        public virtual Node Clone(ICollection<Node> clonedNodes)
        {
            var clone = Instantiate(this);
            clonedNodes.Add(clone);
            return clone;
        }

        public void Abort()
        {
            if (State == NodeState.Running)
            {
                IsStarted = false;
                State = NodeState.Aborted;
                OnAbort();
            }
        }

        protected virtual void OnAbort()
        {
            
        }
    }
}