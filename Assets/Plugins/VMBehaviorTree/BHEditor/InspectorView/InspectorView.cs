#if UNITY_EDITOR
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

namespace VMBehaviorTree.Editor
{
    public partial class InspectorView : VisualElement
    {
        UnityEditor.Editor editor;
        
        private readonly IMGUIContainer container;

        public InspectorView()
        {
            container = new IMGUIContainer();
            Add(container);
            
            style.display = DisplayStyle.None;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void InspectObject(Object obj)
        {
            if (obj == null)
            {
                return;
            }
            
            style.display = DisplayStyle.Flex;
            
            if (editor != null)
            {
                Object.DestroyImmediate(editor);
            }

            editor = UnityEditor.Editor.CreateEditor(obj);
            container.onGUIHandler = () => editor.OnInspectorGUI();
        }
    }
}
#endif