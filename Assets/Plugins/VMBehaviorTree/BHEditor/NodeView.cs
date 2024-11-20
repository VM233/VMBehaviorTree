#if UNITY_EDITOR
using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using VMFramework.Core.Editor;

namespace VMBehaviorTree.Editor
{
    [UxmlElement]
    public sealed partial class NodeView : UnityEditor.Experimental.GraphView.Node
    {
        public readonly Node node;

        private readonly Label descriptionLabel;
        
        public Port InputPort { get; private set; }
        public Port OutputPort { get; private set; }
        
        public event Action<NodeView> OnSelectedEvent;

        public NodeView() : base()
        {
            mainContainer.Insert(0, inputContainer);
            mainContainer.Add(outputContainer);

            descriptionLabel = new Label()
            {
                name = "description-label"
            };
            
            titleContainer.Add(descriptionLabel);
            
            titleButtonContainer.Clear();
        }

        public NodeView(Node node) : this()
        {
            this.node = node;
            
            title = node.name;
            
            style.left = node.position.x;
            style.top = node.position.y;
            
            CreateInputPorts();
            CreateOutputPorts();
            
            node.OnValidateEvent += OnNodeDataChanged;
            
            OnNodeDataChanged(node);
        }

        public void ClearInfo()
        {
            node.OnValidateEvent -= OnNodeDataChanged;
        }

        private void OnNodeDataChanged(Node node)
        {
            descriptionLabel.text = node.GetDescription();
        }

        public void UpdateInRuntime()
        {
            var runningStyle = StyleSheetsUtility.GetNodeViewRunningStyle();
            styleSheets.Remove(runningStyle);
            var failureStyle = StyleSheetsUtility.GetNodeViewFailureStyle();
            styleSheets.Remove(failureStyle);
            var successStyle = StyleSheetsUtility.GetNodeViewSuccessStyle();
            styleSheets.Remove(successStyle);
            var abortedStyle = StyleSheetsUtility.GetNodeViewAbortedStyle();
            styleSheets.Remove(abortedStyle);

            if (node.HasStateUpdatedThisFrame == false && node.State != NodeState.Aborted)
            {
                return;
            }

            styleSheets.Add(node.State switch
            {
                NodeState.Running => runningStyle,
                NodeState.Failure => failureStyle,
                NodeState.Success => successStyle,
                NodeState.Aborted => abortedStyle,
                _ => throw new ArgumentOutOfRangeException()
            });
        }

        private void CreateInputPorts()
        {
            if (node.HasInput == false)
            {
                return;
            }

            InputPort = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Single, typeof(bool));
            InputPort.portName = "";
            inputContainer.Add(InputPort);
        }

        private void CreateOutputPorts()
        {
            if (node.OutputPortType == NodePortType.None)
            {
                return;
            }

            var capacity = node.OutputPortType switch
            {
                NodePortType.Single => Port.Capacity.Single,
                NodePortType.Multiple => Port.Capacity.Multi,
                _ => throw new ArgumentOutOfRangeException()
            };

            OutputPort = InstantiatePort(Orientation.Vertical, Direction.Output, capacity, typeof(bool));
            OutputPort.portName = "";
            outputContainer.Add(OutputPort);
        }

        public override void OnSelected()
        {
            base.OnSelected();
            
            OnSelectedEvent?.Invoke(this);
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            base.BuildContextualMenu(evt);

            evt.menu.AppendAction("Open Script", _ => { node.OpenScriptOfObject(); });
        }
    }
}
#endif