namespace VMBehaviorTree
{
    public partial class WaitNode
    {
        public override string GetDescription()
        {
            return $"Wait for {duration} seconds.";
        }
    }
}