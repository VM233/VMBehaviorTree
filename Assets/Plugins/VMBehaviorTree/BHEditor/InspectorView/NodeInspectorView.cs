#if UNITY_EDITOR
using System.Runtime.CompilerServices;
using UnityEngine.UIElements;

namespace VMBehaviorTree.Editor
{
    [UxmlElement]
    public sealed partial class NodeInspectorView : InspectorView
    {
        private readonly Label nodeNameLabel;
        private readonly Label treeNameLabel;

        public NodeInspectorView() : base()
        {
            styleSheets.Add(StyleSheetsUtility.Load(BTEditorConstants.NODE_INSPECTOR_VIEW_STYLE_SHEET));

            nodeNameLabel = new Label()
            {
                name = "node-name-label"
            };
            Insert(0, nodeNameLabel);
            
            treeNameLabel = new Label()
            {
                name = "tree-name-label",
            };
            Insert(0, treeNameLabel);
            
            treeNameLabel.style.display = DisplayStyle.None;
            nodeNameLabel.style.display = DisplayStyle.None;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void InspectNode(BehaviorTree tree, Node node)
        {
            InspectObject(node);
            
            treeNameLabel.text = tree.name;
            treeNameLabel.style.display = DisplayStyle.Flex;
            nodeNameLabel.text = node.name;
            nodeNameLabel.style.display = DisplayStyle.Flex;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void InspectTree(BehaviorTree tree)
        {
            InspectObject(tree);
            
            treeNameLabel.text = tree.name;
            treeNameLabel.style.display = DisplayStyle.Flex;
            nodeNameLabel.style.display = DisplayStyle.None;
        }
    }
}
#endif