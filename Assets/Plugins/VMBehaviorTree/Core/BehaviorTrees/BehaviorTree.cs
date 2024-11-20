using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Serialization;
using VMFramework.Core.Pools;

namespace VMBehaviorTree
{
    [CreateAssetMenu(menuName = BTConstants.NAME + "/Behavior Tree")]
    public sealed partial class BehaviorTree : ScriptableObject
    {
        public Blackboard blackboard;
        
        public Node rootNode;

        public List<Node> nodes = new();

        public int updateRate = 1;
        
        public NodeState TreeState { get; private set; }
        
        public event Action<BehaviorTree> OnBeforeUpdating;

        public event Action<BehaviorTree> OnAfterUpdating;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Initialize()
        {
            TreeState = NodeState.Running;

            blackboard.Initialize();
            
            var keys = ListPool<IKey>.Default.Get();
            keys.Clear();

            foreach (var node in nodes)
            {
                if (node is not IKeysOwner keysOwner)
                {
                    continue;
                }
                
                keysOwner.GetKeys(keys);

                foreach (var key in keys)
                {
                    key.SetBlackboard(blackboard);
                }
                
                keys.Clear();
            }
            
            keys.ReturnToDefaultPool();
            
            foreach (var node in nodes)
            {
                node.Initialize();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public NodeState Update()
        {
            OnBeforeUpdating?.Invoke(this);
            rootNode.UpdatePerFrame();
            TreeState = rootNode.Update();
            OnAfterUpdating?.Invoke(this);
            return TreeState;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Stop()
        {
            foreach (var node in nodes)
            {
                node.Stop();
            }
        }

        public BehaviorTree Clone()
        {
            var cloneTree = Instantiate(this);
            cloneTree.nodes.Clear();
            cloneTree.rootNode = cloneTree.rootNode.Clone(cloneTree.nodes);
            var blackboard = Instantiate(this.blackboard);
            cloneTree.blackboard = blackboard;
            return cloneTree;
        }
    }
}