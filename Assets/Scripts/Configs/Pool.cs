﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class Pool : MonoBehaviour
{
    [SerializeField] private GameObject[] _items;

    private static Pool _instance;

    private static readonly Dictionary<Type, GameObject> Items = new();

    private static readonly List<MonoBehaviour> PooledObjects = new();

    private void Awake()
    {
        foreach (var item in _items) 
            Items.Add(item.GetType(), item);
    }

    public static T Get<T>(Transform parent = null) where T : MonoBehaviour
    {
        var obj = PooledObjects.Find(o => o.GetType() == typeof(T)) as T;
        if (obj == null)
        {
            foreach (var item in Items)
            {
                if (item.Value.TryGetComponent(out T value))
                {
                    obj = Instantiate(value, parent);
                    obj.gameObject.SetActive(true);
                    return obj;
                }
            }
        }
        else
        {
            obj.gameObject.SetActive(true);
            obj.transform.SetParent(parent);
            PooledObjects.Remove(obj);
        }

        if (obj == null)
            Debug.LogWarning($"Object of type {typeof(T)} doesn't exist in pool");
        
        return obj;
    }

    public static void Release<T>(T obj) where T : MonoBehaviour
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(null);
        obj.transform.SetPositionAndRotation(Vector3.zero, Quaternion.Euler(Vector3.zero));
        
        PooledObjects.Add(obj);
    }
}