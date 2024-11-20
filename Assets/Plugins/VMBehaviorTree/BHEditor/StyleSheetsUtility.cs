#if UNITY_EDITOR
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

namespace VMBehaviorTree.Editor
{
    public static class StyleSheetsUtility
    {
        private static StyleSheet nodeViewRunningStyle;
        private static StyleSheet nodeViewFailureStyle;
        private static StyleSheet nodeViewSuccessStyle;
        private static StyleSheet nodeViewAbortedStyle;
        
        public static StyleSheet Load(string name)
        {
            return Resources.Load<StyleSheet>(BTEditorConstants.STYLE_SHEETS_PATH + name);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static StyleSheet GetNodeViewStyle(NodeState state)
        {
            return state switch
            {
                NodeState.Running => GetNodeViewRunningStyle(),
                NodeState.Failure => GetNodeViewFailureStyle(),
                NodeState.Success => GetNodeViewSuccessStyle(),
                NodeState.Aborted => GetNodeViewAbortedStyle(),
                _ => null
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static StyleSheet GetNodeViewRunningStyle()
        {
            if (nodeViewRunningStyle == null)
            {
                nodeViewRunningStyle = Load(BTEditorConstants.NODE_VIEW_RUNNING_STYLE_SHEET);
            }
            
            return nodeViewRunningStyle;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static StyleSheet GetNodeViewFailureStyle()
        {
            if (nodeViewFailureStyle == null)
            {
                nodeViewFailureStyle = Load(BTEditorConstants.NODE_VIEW_FAILURE_STYLE_SHEET);
            }
            
            return nodeViewFailureStyle;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static StyleSheet GetNodeViewSuccessStyle()
        {
            if (nodeViewSuccessStyle == null)
            {
                nodeViewSuccessStyle = Load(BTEditorConstants.NODE_VIEW_SUCCESS_STYLE_SHEET);
            }
            
            return nodeViewSuccessStyle;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static StyleSheet GetNodeViewAbortedStyle()
        {
            if (nodeViewAbortedStyle == null)
            {
                nodeViewAbortedStyle = Load(BTEditorConstants.NODE_VIEW_ABORTED_STYLE_SHEET);
            }
            
            return nodeViewAbortedStyle;
        }
    }
}
#endif