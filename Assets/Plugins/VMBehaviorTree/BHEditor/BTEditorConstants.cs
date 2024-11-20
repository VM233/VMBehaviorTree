#if UNITY_EDITOR
using System;

namespace VMBehaviorTree.Editor
{
    public static class BTEditorConstants
    {
        public const string EDITOR_WINDOW_PATH = "VMFramework/" + BTConstants.NAME;

        public const string STYLE_SHEETS_PATH = "VMBehaviorTree/StyleSheets/";
        
        public const string BEHAVIOR_TREE_EDITOR_STYLE_SHEET = nameof(BehaviorTreeEditor);

        public const string NODE_VIEW_STYLE_SHEET = "NodeViewStyle";
        
        public const string NODE_VIEW_RUNNING_STYLE_SHEET = "NodeViewRunningStyle";
        
        public const string NODE_VIEW_FAILURE_STYLE_SHEET = "NodeViewFailureStyle";
        
        public const string NODE_VIEW_SUCCESS_STYLE_SHEET = "NodeViewSuccessStyle";
        
        public const string NODE_VIEW_ABORTED_STYLE_SHEET = "NodeViewAbortedStyle";
        
        public const string NODE_INSPECTOR_VIEW_STYLE_SHEET = "NodeInspectorView";
        
        public const string BLACKBOARD_INSPECTOR_VIEW_STYLE_SHEET = "BlackboardInspectorView";
    }
}
#endif