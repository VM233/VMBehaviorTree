using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Sirenix.OdinInspector;

namespace VMBehaviorTree
{
    public sealed class InstanceTypeInfo
    {
        public Type type;
        
        [RequiredListLength(nameof(GetGenericArgumentsLength))]
        public List<InstanceTypeInfo> genericArgumentsInfo = new();

        private int GetGenericArgumentsLength()
        {
            if (type == null)
            {
                return 0;
            }

            if (type.IsGenericTypeDefinition == false)
            {
                return 0;
            }

            return type.GetGenericArguments().Length;
        }
        
        public Type GetInstanceType()
        {
            if (type.IsGenericTypeDefinition)
            {
                var genericArgumentTypes = new Type[genericArgumentsInfo.Count];
                for (int i = 0; i < genericArgumentsInfo.Count; i++)
                {
                    genericArgumentTypes[i] = genericArgumentsInfo[i].GetInstanceType();
                }

                return type.MakeGenericType(genericArgumentTypes);
            }
            
            return type;
        }
    }
}