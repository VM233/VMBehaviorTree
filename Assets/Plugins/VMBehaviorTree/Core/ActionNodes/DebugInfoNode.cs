using System.Collections.Generic;
using UnityEngine;

namespace VMBehaviorTree
{
    [NodeContents(BTConstants.ACTION_DEBUG_NODES, NAME)]
    public sealed partial class DebugInfoNode : ActionNode, IKeysOwner
    {
        public const string NAME = "Debug Info";
        
        private enum LogLevel
        {
            Info,
            Warning,
            Error
        }

        [SerializeField]
        private LogLevel logLevel;
        
        [SerializeField]
        private KeyOrFixed<string> message = new();
        
        protected override void OnUpdateStart()
        {
            
        }

        protected override NodeState OnUpdate()
        {
            var messageValue = message.GetValue();
            
            switch (logLevel)
            {
                case LogLevel.Info:
                    Debug.Log(messageValue);
                    break;
                case LogLevel.Warning:
                    Debug.LogWarning(messageValue);
                    break;
                case LogLevel.Error:
                    Debug.LogError(messageValue);
                    break;
            }
            
            return NodeState.Success;
        }

        protected override void OnUpdateStop()
        {
            
        }

        public void GetKeys(ICollection<IKey> keys)
        {
            keys.Add(message);
        }
    }
}