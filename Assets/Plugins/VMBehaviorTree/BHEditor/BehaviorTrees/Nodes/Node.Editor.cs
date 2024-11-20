#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEngine;

namespace VMBehaviorTree
{
    public partial class Node
    {
        [HideInInspector]
        public Vector2 position;
        
        public abstract bool HasInput { get; }
        
        public abstract NodePortType OutputPortType { get; }

        public event Action<Node> OnValidateEvent;

        public virtual void SortChildren(Comparison<Node> comparison)
        {
            
        }

        public virtual string GetDescription()
        {
            return string.Empty;
        }

        private void OnValidate()
        {
            OnValidateEvent?.Invoke(this);
        }
    }
}
#endif