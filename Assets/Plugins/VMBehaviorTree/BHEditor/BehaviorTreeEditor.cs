#if UNITY_EDITOR
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using VMFramework.Core;
using VMFramework.Core.Editor;

namespace VMBehaviorTree.Editor
{
    public sealed class BehaviorTreeEditor : EditorWindow
    {
        private VisualElement viewContainer;
        
        private BehaviorTreeView treeView;
        
        private NodeInspectorView nodeInspectorView;
        private BlackboardInspectorView blackboardInspectorView;
        
        private NodeSearcher nodeSearcher;
        
        private BehaviorTree behaviorTree;
        
        private BehaviorTreeRunner behaviorTreeRunner;

        private static BehaviorTreeEditor InitWindow()
        {
            BehaviorTreeEditor window = GetWindow<BehaviorTreeEditor>();
            window.titleContent = new GUIContent("Behavior Tree");
            window.minSize = new Vector2(750, 500);
            return window;
        }
        
        [MenuItem(BTEditorConstants.EDITOR_WINDOW_PATH)]
        public static void Open()
        {
            Open(behaviorTree: null);
        }
        
        public static void Open(BehaviorTree behaviorTree)
        {
            var window = InitWindow();

            if (behaviorTree != null)
            {
                window.SetBehaviorTree(behaviorTree);
            }
            else
            {
                window.OnSelectionChange();
            }
        }

        public static void Open(BehaviorTreeRunner behaviorTreeRunner)
        {
            var window = InitWindow();

            if (behaviorTreeRunner != null)
            {
                window.SetBehaviorTreeRunner(behaviorTreeRunner);
            }
            else
            {
                window.OnSelectionChange();
            }
        }

        private void CreateGUI()
        {
            VisualElement root = rootVisualElement;

            var styleSheet = StyleSheetsUtility.Load(BTEditorConstants.BEHAVIOR_TREE_EDITOR_STYLE_SHEET);
            root.styleSheets.Add(styleSheet);

            var nodeViewStyleSheet = StyleSheetsUtility.Load(BTEditorConstants.NODE_VIEW_STYLE_SHEET);
            root.styleSheets.Add(nodeViewStyleSheet);

            viewContainer = new VisualElement()
            {
                style = { flexGrow = 1 }
            };
            root.Add(viewContainer);

            CreateTreeView();
            CreateInspectorView();
            CreateToolbar();
            CreateNodeSearchWindow();

            if (behaviorTree == null && behaviorTreeRunner == null)
            {
                OnSelectionChange();
            }
            else
            {
                OnTreeChanged();
            }
            
            Undo.undoRedoPerformed += OnUndoRedo;
            
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        private void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.EnteredEditMode)
            {
                OnTreeChanged();
            }
        }

        private void OnDestroy()
        {
            PreChangeTree();
            
            Undo.undoRedoPerformed -= OnUndoRedo;
            
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
        }

        private void OnUndoRedo()
        {
            OnTreeChanged();
        }

        private void CreateTreeView()
        {
            treeView = new BehaviorTreeView();
            
            treeView.OnNodeSelected += (treeView, nodeView) =>
            {
                nodeInspectorView.InspectNode(treeView.Tree, nodeView.node);
            };
            
            viewContainer.Insert(0, treeView);
        }

        private void CreateInspectorView()
        {
            nodeInspectorView = new NodeInspectorView();
            
            viewContainer.Add(nodeInspectorView);
            
            blackboardInspectorView = new BlackboardInspectorView();
            
            viewContainer.Add(blackboardInspectorView);
        }
        
        private void CreateToolbar()
        {
            var toolbar = new Toolbar();

            var saveButton = new ToolbarButton(() =>
            {
                if (EditorApplication.isPlaying)
                {
                    Debug.LogWarning("Cannot save while in play mode.");
                    return;
                }
                
                var behaviorTree = GetSelectedTree(out _);
                behaviorTree.SetEditorDirty();
                behaviorTree.nodes.SetEditorDirty();
                behaviorTree.blackboard.SetEditorDirty();
                
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            })
            {
                text = "Save"
            };

            var nodeInspectorButton = new ToolbarButton(() =>
            {
                nodeInspectorView.ToggleDisplay();
            })
            {
                text = "Node Inspector"
            };
            
            var blackboardInspectorButton = new ToolbarButton(() =>
            {
                blackboardInspectorView.ToggleDisplay();
            })
            {
                text = "Blackboard Inspector"
            };

            var treeSettingButton = new ToolbarButton(() =>
            {
                var behaviorTree = GetSelectedTree(out _);
                if (behaviorTree != null)
                {
                    nodeInspectorView.InspectTree(behaviorTree);
                }
            })
            {
                text = "Tree Settings"
            };
            
            toolbar.Add(saveButton);
            toolbar.Add(nodeInspectorButton);
            toolbar.Add(blackboardInspectorButton);
            toolbar.Add(treeSettingButton);
            
            rootVisualElement.Insert(0, toolbar);
        }

        private void CreateNodeSearchWindow()
        {
            nodeSearcher = CreateInstance<NodeSearcher>();

            nodeSearcher.OnSelected += (type, pos) =>
            {
                Vector2 worldMousePos = rootVisualElement.ChangeCoordinatesTo(rootVisualElement.parent, pos - position.position);
                Vector2 localMousePos = treeView.WorldToLocal(worldMousePos);
                    
                treeView.CreateNode(type, localMousePos);
            };
            
            treeView.nodeCreationRequest += context =>
            {
                SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), nodeSearcher);
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetBehaviorTree(BehaviorTree behaviorTree)
        {
            this.behaviorTree = behaviorTree;
            this.behaviorTreeRunner = null;
            
            OnTreeChanged();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetBehaviorTreeRunner(BehaviorTreeRunner behaviorTreeRunner)
        {
            this.behaviorTreeRunner = behaviorTreeRunner;
            this.behaviorTree = null;
            
            OnTreeChanged();
        }

        private void PreChangeTree()
        {
            if (behaviorTreeRunner != null)
            {
                behaviorTreeRunner.OnTreeStarted -= OnTreeRunnerStarted;
            }
        }

        private void OnTreeRunnerStarted(BehaviorTreeRunner runner)
        {
            OnTreeChanged();
        }

        private void OnTreeChanged()
        {
            var behaviorTree = GetSelectedTree(out bool fromRunner);

            if (fromRunner)
            {
                behaviorTreeRunner.OnTreeStarted -= OnTreeRunnerStarted;
                behaviorTreeRunner.OnTreeStarted += OnTreeRunnerStarted;
            }
            
            treeView.PopulateView(behaviorTree);
            blackboardInspectorView.InspectBlackboard(behaviorTree.blackboard);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private BehaviorTree GetSelectedTree(out bool fromRunner)
        {
            if (behaviorTreeRunner != null)
            {
                fromRunner = true;
                
                if (EditorApplication.isPlaying)
                {
                    return behaviorTreeRunner.BehaviorTreeInstance;
                }
                return behaviorTreeRunner.behaviorTree;
            }
            
            fromRunner = false;
            return behaviorTree;
        }

        private void OnSelectionChange()
        {
            if (Selection.activeObject is BehaviorTree behaviorTree)
            {
                SetBehaviorTree(behaviorTree);
            }
            else if (Selection.activeGameObject != null &&
                     Selection.activeGameObject.TryGetComponent(out BehaviorTreeRunner behaviorTreeRunner))
            {
                SetBehaviorTreeRunner(behaviorTreeRunner);
            }
        }
    }
}

#endif