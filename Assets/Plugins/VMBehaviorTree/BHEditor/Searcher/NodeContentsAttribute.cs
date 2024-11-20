using System;

namespace VMBehaviorTree
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class NodeContentsAttribute : Attribute
    {
        public string Folder;
        
        public string Name;

        public NodeContentsAttribute(string name)
        {
            Name = name;
        }
        
        public NodeContentsAttribute(string folder, string name)
        {
            Folder = folder;
            Name = name;
        }
    }
}