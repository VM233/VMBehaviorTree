using System;
using System.Runtime.CompilerServices;
using Sirenix.OdinInspector;
using UnityEngine;

namespace VMBehaviorTree
{
    public sealed partial class BehaviorTreeRunner : MonoBehaviour
    {
        public BehaviorTree behaviorTree;

        public bool initializeOnStart = true;
        
        public BehaviorTreeUpdateType updateType;

        [ShowInInspector, ReadOnly]
        public BehaviorTree BehaviorTreeInstance { get; private set; }

        [ShowInInspector, ReadOnly]
        private bool isInitialized = false;
        
        [ShowInInspector, ReadOnly]
        private int currentTick = 0;

        public event Action<BehaviorTreeRunner> OnTreeStarted; 

        private void Start()
        {
            if (initializeOnStart)
            {
                InitializeTree();
            }
        }

        private void Update()
        {
            if (isInitialized && updateType == BehaviorTreeUpdateType.OnUpdate)
            {
                UpdateTree();
            }
        }

        private void LateUpdate()
        {
            if (isInitialized && updateType == BehaviorTreeUpdateType.OnLateUpdate)
            {
                UpdateTree();
            }
        }

        private void FixedUpdate()
        {
            if (isInitialized && updateType == BehaviorTreeUpdateType.OnFixedUpdate)
            {
                UpdateTree();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void InitializeTree()
        {
            BehaviorTreeInstance = behaviorTree.Clone();
            isInitialized = true;
            
            BehaviorTreeInstance.Initialize();
            
            OnTreeStarted?.Invoke(this);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void UpdateTree()
        {
            currentTick++;
            if (currentTick >= BehaviorTreeInstance.updateRate)
            {
                BehaviorTreeInstance.Update();
                currentTick = 0;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void StopTree()
        {
            BehaviorTreeInstance.Stop();
        }

        private void OnDestroy()
        {
            StopTree();
        }
    }
}