namespace VMBehaviorTree
{
    [NodeContents(BTConstants.COMPOSITE_NODES, NAME)]
    public class SelectorNode : CompositeNode
    {
        private const string NAME = "Selector";

        private int currentIndex = 0;

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

            if (childState is NodeState.Success or NodeState.Running)
            {
                return childState;
            }
            
            currentIndex++;
            
            if (currentIndex >= children.Count)
            {
                return NodeState.Failure;
            }
            
            return NodeState.Running;
        }

        protected override void OnUpdateStop()
        {
            
        }
    }
}