using System.Collections.Generic;
using UnityEngine;

namespace Echobay.PoolSystem
{
    public abstract class Pool<T> where T : Component
    {
        public IReadOnlyList<T> PoolList => _pool;

        protected readonly T Prefab;
        protected readonly Transform Container;

        private readonly int _capacity;
        private readonly List<T> _pool;

        public Pool(T prefab, Transform container, int capacity)
        {
            Prefab = prefab;
            Container = container;

            _capacity = capacity;
            _pool = new(capacity);
        }

        public void CreatePool()
        {
            for (int i = 0; i < _capacity; i++)
            {
                T obj = CreateObject();
                _pool.Add(obj);
                obj.gameObject.SetActive(false);
            }
        }

        public T GetInactiveObject()
        {
            T freeObj = null;

            foreach (T pool in _pool)
            {
                if (pool.gameObject.activeSelf) continue;
                freeObj = pool;
                break;
            }

            if (freeObj == null)
            {
                freeObj = CreateObject();
                _pool.Add(freeObj);
            }

            return freeObj;
        }

        public void Clear()
        {
            foreach (T pool in _pool)
            {
                Object.Destroy(pool.gameObject);
            }

            _pool.Clear();
        }

        protected abstract T CreateObject();
    }
}