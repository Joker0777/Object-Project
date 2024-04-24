using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : MonoBehaviour
{
    private List<T> _objectPool;
    private T _object;
    private Transform _parentTransform;
    private int _poolSize;
    private string _objectTag;

    public string PrefabTag {  get { return _objectTag; } }
    
    public ObjectPool(T @object, int poolSize, Transform parentTransform, string tag)
    {
        _object = @object;
        _parentTransform = parentTransform;
        _poolSize = poolSize;
        _objectTag = tag;

        CreatePool();
    }

      public T GetObject() 
      {
        foreach (T obj in _objectPool)
        {
            if (!obj.gameObject.activeInHierarchy)
            {               
                obj.gameObject.SetActive(true);
                return obj;
            }
        }
          T newObject = CreateObject(true);
          return newObject;
    }

    public void ReturnObject(T obj)
    {
        obj.gameObject.SetActive(false);
    }

    private void CreatePool()
    {
        _objectPool = new List<T>();

        for (int i = 0; i < _poolSize; i++)
        {
            CreateObject(false);
        }
    }

    private T CreateObject(bool isActive)
    {
        T nextObject = Object.Instantiate(_object, _parentTransform);
        nextObject.gameObject.SetActive(true);
        _objectPool.Add(nextObject);
        nextObject.gameObject.SetActive(isActive);
        return nextObject;
    }


}
