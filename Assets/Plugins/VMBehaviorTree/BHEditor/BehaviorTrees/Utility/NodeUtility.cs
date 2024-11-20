#if UNITY_EDITOR
using System;
using System.Runtime.CompilerServices;
using VMFramework.Core.Editor;

namespace VMBehaviorTree.Editor
{
    public static class NodeUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AddChildInEditor(this Node parent, Node child)
        {
            if (parent == null)
            {
                throw new NullReferenceException(nameof(parent));
            }
            
            parent.AddChild(child);
            
            parent.SetEditorDirty();
            child.SetEditorDirty();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RemoveChildInEditor(this Node parent, Node child)
        {
            if (parent == null)
            {
                throw new NullReferenceException(nameof(parent));
            }
            
            parent.RemoveChild(child);
            
            parent.SetEditorDirty();
            child.SetEditorDirty();
        }
    }
}
#endif