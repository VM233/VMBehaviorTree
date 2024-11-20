#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using VMFramework.Core;

namespace VMBehaviorTree.Editor
{
    public sealed class NodeSearcher : ScriptableObject, ISearchWindowProvider
    {
        public event Action<Type, Vector2> OnSelected; 
        
        public void Init()
        {
            
        }

        List<SearchTreeEntry> ISearchWindowProvider.CreateSearchTree(SearchWindowContext context)
        {
            var tree = new List<SearchTreeEntry>()
            {
                new SearchTreeGroupEntry(new GUIContent("Add Node"), 0),
            };

            var pathTree = new StringPathTree<Type>();
            
            foreach (var type in typeof(Node).GetDerivedInstantiableClasses(false))
            {
                if (type.TryGetAttribute(false, out NodeContentsAttribute nodeContents))
                {
                    var path = nodeContents.Folder + "/" + nodeContents.Name;
                    
                    pathTree.Add(path, type);
                }
            }

            foreach (var pathTreeNode in pathTree.Root.PreorderTraverse(false))
            {
                if (pathTreeNode.Children.Count != 0)
                {
                    tree.Add(new SearchTreeGroupEntry(new GUIContent(pathTreeNode.PathPart), pathTreeNode.Depth));
                }
                else
                {
                    tree.Add(new SearchTreeEntry(new GUIContent(pathTreeNode.PathPart))
                    {
                        userData = pathTreeNode.data,
                        level = pathTreeNode.Depth
                    });
                }
            }

            return tree;
        }

        bool ISearchWindowProvider.OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
        {
            if (searchTreeEntry.userData is Type type)
            {
                OnSelected?.Invoke(type, context.screenMousePosition);
                return true;
            }
            
            return false;
        }
    }
}
#endif