#if UNITY_EDITOR
using System.Runtime.CompilerServices;
using UnityEngine.UIElements;

namespace VMBehaviorTree.Editor
{
    [UxmlElement]
    public sealed partial class BlackboardInspectorView : InspectorView
    {
        private readonly Label titleLabel;

        public BlackboardInspectorView()
        {
            styleSheets.Add(StyleSheetsUtility.Load(BTEditorConstants.BLACKBOARD_INSPECTOR_VIEW_STYLE_SHEET));
            
            titleLabel = new Label()
            {
                name = "title-label",
                text = "Blackboard"
            };
            
            Insert(0, titleLabel);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void InspectBlackboard(Blackboard blackboard)
        {
            InspectObject(blackboard);
        }
    }
}
#endif