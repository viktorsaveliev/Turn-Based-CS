using UnityEngine;

namespace Echobay.PoolSystem
{
    public class ObjectPool<T> : Pool<T> where T : Component
    {
        public ObjectPool(T prefab, Transform container, int capacity) : base(prefab, container, capacity)
        {
        }

        protected override T CreateObject() => Object.Instantiate(Prefab, Container);
    }
}