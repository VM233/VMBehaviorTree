using VMFramework.Core;

namespace VMBehaviorTree
{
    [NodeContents(BTConstants.COMPOSITE_NODES, NAME)]
    public class RandomSelectorNode : SelectorNode
    {
        private const string NAME = "Random Selector";

        protected override void OnUpdateStart()
        {
            base.OnUpdateStart();
            
            children.Shuffle();
        }
    }
}