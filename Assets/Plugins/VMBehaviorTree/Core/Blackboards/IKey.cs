namespace VMBehaviorTree
{
    public interface IKey<TValue> : IKey
    {
        public TValue GetValue();

        public void SetValue(TValue value);

        object IKey.GetObjectValue()
        {
            return GetValue();
        }

        void IKey.SetObjectValue(object value)
        {
            SetValue((TValue)value);
        }
    }

    public interface IKey
    {
        public void SetBlackboard(Blackboard blackboard);
        
        public object GetObjectValue();

        public void SetObjectValue(object value);
    }
}