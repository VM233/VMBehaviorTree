#if UNITY_EDITOR
using System;

namespace VMBehaviorTree
{
    public partial class DebugInfoNode
    {
        public override string GetDescription()
        {
            return $"Log {logLevel} : {message}";
        }
    }
}
#endif