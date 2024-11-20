using System;

namespace VMBehaviorTree
{
    public class KeyInfo<T> : IKeyInfo
    {
        public string keyID;
        public T initialValue;

        string IKeyInfo.KeyID => keyID;

        Type IKeyInfo.Type => typeof(T);

        object IKeyInfo.InitialValue => initialValue;
    }
}