#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using VMFramework.Core.Editor;
using VMFramework.Core.Pools;
using Object = UnityEngine.Object;

namespace VMBehaviorTree.Editor
{
    [UxmlElement]
    public sealed partial class BehaviorTreeView : GraphView
    {
        private Label behaviorTreeNameLabel;
        
        public BehaviorTree Tree { get; private set; }

        private readonly Dictionary<Node, NodeView> nodeViews = new();
        
        public IReadOnlyDictionary<Node, NodeView> NodeViews => nodeViews;

        public event Action<BehaviorTreeView, NodeView> OnNodeSelected;

        private readonly Action<NodeView> onNodeViewSelectedFunc;
        
        public BehaviorTreeView() : base()
        {
            onNodeViewSelectedFunc = OnNodeViewSelected;
            
            style.flexGrow = 1;
            
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            var grid = new GridBackground()
            {
                name = "GridBackground"
            };
            grid.StretchToParentSize();
            Insert(0, grid);
        }

        public void PopulateView(BehaviorTree behaviorTree)
        {
            ClearInfo();
            
            Tree = behaviorTree;

            if (behaviorTree == null)
            {
                return;
            }
            
            graphViewChanged += OnGraphViewChanged;
            viewTransformChanged += OnViewTransformChanged;
            
            behaviorTree.OnAfterUpdating += OnBehaviorTreeUpdated;
            
            InitializeMissingAssets(behaviorTree);

            foreach (var node in Tree.nodes)
            {
                CreateNodeView(behaviorTree.blackboard, node);
            }
            
            var children = new List<Node>();

            foreach (var node in Tree.nodes)
            {
                node.GetChildren(children);

                if (children.Count == 0)
                {
                    continue;
                }
                
                var parentNodeView = nodeViews[node];

                foreach (var child in children)
                {
                    var childNodeView = nodeViews[child];
                    
                    var edge = parentNodeView.OutputPort.ConnectTo(childNodeView.InputPort);
                    
                    AddElement(edge);
                }

                children.Clear();
            }

            if (behaviorTree.viewScale != Vector3.zero)
            {
                UpdateViewTransform(behaviorTree.viewPosition, behaviorTree.viewScale);
            }
        }

        private static void InitializeMissingAssets(BehaviorTree behaviorTree)
        {
            if (behaviorTree.rootNode == null)
            {
                behaviorTree.rootNode = behaviorTree.CreateNode(typeof(RootNode), true);
            }

            if (behaviorTree.blackboard == null)
            {
                behaviorTree.CreateBlackboard(true);
            }
        }

        private void ClearInfo()
        {
            if (Tree != null)
            {
                Tree.OnAfterUpdating -= OnBehaviorTreeUpdated;
            }
            
            graphViewChanged -= OnGraphViewChanged;
            viewTransformChanged -= OnViewTransformChanged;

            var nodeViewsList = ListPool<NodeView>.Default.Get();
            nodeViewsList.Clear();
            nodeViewsList.AddRange(nodeViews.Values);
            foreach (var nodeView in nodeViewsList)
            {
                DeleteNodeView(nodeView);
            }
            nodeViewsList.ReturnToDefaultPool();
            
            DeleteElements(graphElements);
            
            nodeViews.Clear();
        }

        private void OnBehaviorTreeUpdated(BehaviorTree behaviorTree)
        {
            foreach (var nodeView in nodeViews.Values)
            {
                nodeView.UpdateInRuntime();
            }
        }
        
        private void OnViewTransformChanged(GraphView graphview)
        {
            Tree.viewPosition = viewTransform.position;
            Tree.viewScale = viewTransform.scale;
            Tree.SetEditorDirty();
        }

        #region On Graph View Changed

        private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange)
        {
            if (EditorApplication.isPlaying) 
            {
                return graphViewChange;
            }
            
            OnElementsToRemove(graphViewChange.elementsToRemove);
            OnEdgesToCreate(graphViewChange.edgesToCreate);
            OnMovedElements(graphViewChange.movedElements);
            
            return graphViewChange;
        }

        private void OnMovedElements(IEnumerable<GraphElement> elements)
        {
            if (elements == null)
            {
                return;
            }
            
            var movedNodeViews = new List<NodeView>();

            foreach (var element in elements)
            {
                if (element is not NodeView nodeView)
                {
                    continue;
                }

                movedNodeViews.Add(nodeView);
            }
            
            var changedObjects = new HashSet<Object>();

            foreach (var nodeView in movedNodeViews)
            {
                changedObjects.Add(nodeView.node);
                
                var nodeParent = nodeView.node.GetParent();

                if (nodeParent != null)
                {
                    changedObjects.Add(nodeParent);
                }
            }

            Undo.RecordObjects(changedObjects.ToArray(), "Behavior Tree (Move Node)");

            foreach (var nodeView in movedNodeViews)
            {
                var node = nodeView.node;
                var newPos = nodeView.GetPosition();
                
                node.position = newPos.position;
                node.SetEditorDirty();
            }
            
            var parents = new HashSet<Node>();

            foreach (var nodeView in movedNodeViews)
            {
                var parent = nodeView.node.GetParent();

                if (parent == null)
                {
                    continue;
                }
                
                parents.Add(parent);
            }

            var children = new List<Node>();
            var angles = new Dictionary<Node, float>();
            foreach (var parent in parents)
            {
                parent.GetChildren(children);

                if (children.Count <= 1)
                {
                    children.Clear();
                    continue;
                }

                foreach (var child in children)
                {
                    var childNodeView = nodeViews[child];
                    var parentNodeView = nodeViews[parent];

                    var childViewPosition = childNodeView.GetPosition().center;
                    var parentViewPosition = parentNodeView.GetPosition().center;
                    
                    var direction = childViewPosition - parentViewPosition;
                    direction.y = -direction.y;

                    var angle = Vector2.SignedAngle(Vector2.down, direction);

                    angles.Add(child, angle);
                }

                parent.SortChildren((leftChild, rightChild) => angles[leftChild].CompareTo(angles[rightChild]));
                
                children.Clear();
                angles.Clear();
                
                parent.SetEditorDirty();
            }
        }

        private void OnEdgesToCreate(List<Edge> edges)
        {
            if (edges == null)
            {
                return;
            }

            foreach (var edge in edges)
            {
                if (edge.output.node is not NodeView parentNodeView || edge.input.node is not NodeView childNodeView)
                {
                    continue;
                }
                
                Undo.RecordObject(parentNodeView.node, "Behavior Tree (Add Child)");
                parentNodeView.node.AddChildInEditor(childNodeView.node);
            }
        }

        private void OnElementsToRemove(List<GraphElement> elements)
        {
            if (elements == null)
            {
                return;
            }

            foreach (var element in elements)
            {
                if (element is not Edge edge)
                {
                    continue;
                }

                if (edge.output.node is not NodeView parentNodeView || edge.input.node is not NodeView childNodeView)
                {
                    continue;
                }

                Undo.RecordObject(parentNodeView.node, "Behavior Tree (Remove Child)");
                parentNodeView.node.RemoveChildInEditor(childNodeView.node);
            }
            
            foreach (var element in elements)
            {
                if (element is not NodeView nodeView)
                {
                    continue;
                }
                
                DeleteNode(nodeView);
            }
        }

        #endregion

        private void DeleteNode(NodeView nodeView)
        {
            var node = nodeView.node;
                    
            DeleteNodeView(nodeView);
            
            Tree.DeleteNode(nodeView.node);
        }

        private void DeleteNodeView(NodeView nodeView)
        {
            nodeView.ClearInfo();
            
            nodeViews.Remove(nodeView.node);
            
            nodeView.OnSelectedEvent -= onNodeViewSelectedFunc;
        }

        public void CreateNode(Type type, Vector2 position)
        {
            var node = Tree.CreateNode(type, true);
            node.position = position;
            CreateNodeView(Tree.blackboard, node);
        }

        private void CreateNodeView(Blackboard blackboard, Node node)
        {
            var nodeView = new NodeView(node);
            AddElement(nodeView);
            
            nodeView.OnSelectedEvent += onNodeViewSelectedFunc;
            
            var keys = ListPool<IKey>.Default.Get();
            keys.Clear();
            
            if (node is IKeysOwner keysOwner)
            {
                keysOwner.GetKeys(keys);

                foreach (var key in keys)
                {
                    key.SetBlackboard(blackboard);
                }
            
                keys.ReturnToDefaultPool();
            }
            
            nodeViews.Add(node, nodeView);
        }

        private void OnNodeViewSelected(NodeView nodeView)
        {
            OnNodeSelected?.Invoke(this, nodeView);
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            return ports.Where(p => p.direction != startPort.direction && p.node != startPort.node).ToList();
        }
    }
}
#endif