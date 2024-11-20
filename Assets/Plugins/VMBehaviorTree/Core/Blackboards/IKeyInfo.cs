using System;

namespace VMBehaviorTree
{
    public interface IKeyInfo
    {
        public string KeyID { get; }
        
        public Type Type { get; }
        
        public object InitialValue { get; }
    }
}