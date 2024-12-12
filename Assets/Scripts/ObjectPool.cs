using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : MonoBehaviour
{
    public int amountToPool = 10;

    private readonly T prefab;
    private readonly List<T> pooledObjects = new();
    private readonly GameObject poolParent;

    public ObjectPool(T prefab, GameObject parent)
    {
        this.prefab = prefab;

        poolParent = new($"{prefab.name} Pool");

        for (int i = 0; i < amountToPool; i++)
        {
            AddPooledObject();
        }
    }

    private void AddPooledObject()
    {
        T obj = GameObject.Instantiate(prefab, poolParent.transform);
        obj.gameObject.SetActive(false);
        pooledObjects.Add(obj);
    }

    public T GetPooledObject()
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].gameObject.activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }

        AddPooledObject();
        return pooledObjects[^1];
    }
}
