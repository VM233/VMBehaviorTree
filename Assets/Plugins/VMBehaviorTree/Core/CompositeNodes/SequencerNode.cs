namespace VMBehaviorTree
{
    [NodeContents(BTConstants.COMPOSITE_NODES, NAME)]
    public class SequencerNode : CompositeNode
    {
        private const string NAME = "Sequencer";
        
        private int currentIndex;

        protected override void OnUpdateStart()
        {
            currentIndex = 0;
        }

        protected override NodeState OnUpdate()
        {
            if (children.Count == 0)
            {
                return NodeState.Success;
            }
            
            var child = children[currentIndex];
            var childState = child.Update();

            if (childState is NodeState.Failure or NodeState.Running)
            {
                return childState;
            }
            
            currentIndex++;

            if (currentIndex >= children.Count)
            {
                return NodeState.Success;
            }
            
            return NodeState.Running;
        }
        
        protected override void OnUpdateStop()
        {
            
        }
    }
}