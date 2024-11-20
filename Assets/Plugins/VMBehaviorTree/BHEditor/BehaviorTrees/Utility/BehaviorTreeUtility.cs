#if UNITY_EDITOR
using System;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using VMFramework.Core;
using VMFramework.Core.Editor;

namespace VMBehaviorTree.Editor
{
    public static class BehaviorTreeUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Blackboard CreateBlackboard(this BehaviorTree tree, bool save)
        {
            var blackboard = ScriptableObject.CreateInstance<Blackboard>();
            blackboard.name = "Blackboard";
            tree.blackboard = blackboard;
            
            AssetDatabase.AddObjectToAsset(blackboard, tree);
            
            if (save)
            {
                tree.EnforceSave();
            }
            else
            {
                tree.SetEditorDirty();
            }
            
            return blackboard;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Node CreateNode(this BehaviorTree tree, Type type, bool save)
        {
            var node = (Node)ScriptableObject.CreateInstance(type);

            var nodeName = type.Name;
            if (type.TryGetAttribute(false, out NodeContentsAttribute nodeContentsAttribute))
            {
                nodeName = nodeContentsAttribute.Name;
            }
            
            node.name = nodeName;
            
            tree.nodes.Add(node);
            
            AssetDatabase.AddObjectToAsset(node, tree);

            if (save)
            {
                tree.EnforceSave();
            }
            else
            {
                tree.SetEditorDirty();
            }
            
            return node;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool DeleteNode(this BehaviorTree tree, Node node)
        {
            if (node == null)
            {
                return false;
            }

            if (tree.nodes.Contains(node) == false)
            {
                return false;
            }
            
            tree.nodes.Remove(node);
            
            AssetDatabase.RemoveObjectFromAsset(node);
            tree.SetEditorDirty();
            
            AssetDatabase.SaveAssets();
            
            return true;
        }
    }
}
#endif