using UnityEngine;

namespace VMBehaviorTree
{
    public abstract partial class ConditionDecorator : DecoratorNode
    {
        [SerializeField]
        private bool inversedCondition;

        protected override NodeState OnUpdate()
        {
            var conditionMet = MeetsCondition();

            if (inversedCondition)
            {
                conditionMet = !conditionMet;
            }

            if (conditionMet)
            {
                return child.Update();
            }

            return NodeState.Failure;
        }
        
        protected abstract bool MeetsCondition();
    }
}