using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPoolSystem<T> where T : MonoBehaviour
{
    private Dictionary<string, ObjectPool<T>> _objectPools;

    private static ObjectPoolSystem<T> _instance;

    public static ObjectPoolSystem<T> Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new ObjectPoolSystem<T>();
            }
            return _instance;
        }
    }

    public ObjectPoolSystem()
    {
        _objectPools = new Dictionary<string, ObjectPool<T>>();
    }

    public void AddPool(int size, string name, T @object, Transform parent)
    {
        if (@object != null && !_objectPools.ContainsKey(name))
        {
            _objectPools.Add(name,new ObjectPool<T>(@object, size, parent, name));
        }
        return;
    }

    public void RemovePool(string name)
    {
        if (_objectPools.TryGetValue(name, out ObjectPool<T> _))
            {
               _objectPools.Remove(name);
            }
        return;
    }

    public T GetObject(string type)
    {
        T obj = null;

        if (_objectPools.TryGetValue(type, out ObjectPool<T> value))
        {
            obj = value.GetObject();
        }
      

        return obj;
    }
}
