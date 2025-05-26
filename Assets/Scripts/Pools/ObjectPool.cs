using System;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    public class ObjectPool<T> : MonoBehaviour, IPool where T : Component
    {
        [SerializeField] private T _prefab;
        [SerializeField] protected int _amount;
        
        private readonly Stack<T> _pool = new();

        public Stack<T> Pool => _pool;
        
        public Type Type => typeof(T);
        
        public void CreateObjects(int count)
        {
            for (int i = 0; i < count; i++)
            {
                var instance = CreateObject();
                _pool.Push(instance);
            }
        }

        private T CreateObject()
        {
            var instance = Instantiate(_prefab, transform);
            SetNumber(instance);
            instance.gameObject.SetActive(false);
            return instance;
        }
        
        public T Get()
        {
            if (_pool.Count == 0)
            {
                CreateObjects(_amount);
            }

            T obj = _pool.Pop();
            obj.gameObject.SetActive(true);
            return obj;
        }

        public void Return(T obj)
        {
            SetNumber(obj);
            obj.gameObject.SetActive(false);
            obj.transform.SetParent(transform);
            _pool.Push(obj);
            Debug.Log($"{obj.name} was returned");
        }

        private void SetNumber(T obj)
        {
            obj.name = $"{_prefab.name}-{_pool.Count}";
        }
    }
}