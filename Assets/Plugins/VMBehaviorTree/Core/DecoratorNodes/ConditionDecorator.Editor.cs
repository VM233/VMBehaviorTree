#if UNITY_EDITOR
using System;

namespace VMBehaviorTree
{
    public partial class ConditionDecorator
    {
        public override string GetDescription()
        {
            var description = name;

            if (inversedCondition)
            {
                description += " == false";
            }
            else
            {
                description += " == true";
            }

            return description;
        }
    }
}
#endif