using System;
using System.Collections.Generic;
using Common;
using UnityEngine;

public class PoolsKeeper : MonoBehaviour
{
    [SerializeField] private List<MonoBehaviour> _pools;

    private Dictionary<Type, IPool> _runtimePools = new();

    private void Awake()
    {
        foreach (var pool in _pools)
        {
            // сделать добавление в стэк всех дочерних элементов и добивку до нужного кол-ва
        }
    }

    [EditorButton("Create Pools")]
    private void Create()
    {
        _runtimePools = new Dictionary<Type, IPool>();
        foreach (var mono in _pools)
        {
            if (mono is not IPool pool)
            {
                Debug.LogError($"Pool {mono.name} does not implement IPool.");
                continue;
            }

            _runtimePools[pool.Type] = pool;
            Debug.Log($"Pools was created.");
        }
    }

    public ObjectPool<T> GetPool<T>() where T : Component
    {
        if (_runtimePools.TryGetValue(typeof(T), out var pool))
            return pool as ObjectPool<T>;

        Debug.LogError($"Pool for type {typeof(T)} not found.");
        return null;
    }
}