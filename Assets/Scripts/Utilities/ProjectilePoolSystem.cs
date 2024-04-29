using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

public class ProjectilePoolSystem
{
    private Dictionary<string, ObjectPool<Projectile>> _projectilePools;

    private static ProjectilePoolSystem _instance;

    public static ProjectilePoolSystem Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new ProjectilePoolSystem();
            }
            return _instance;
        }
    }

    public ProjectilePoolSystem()
    {
        _projectilePools = new Dictionary<string, ObjectPool<Projectile>>();
    }

    public void AddPool(int size, string name, Projectile projectile, Transform parent)
    {
        if (projectile != null && !_projectilePools.ContainsKey(name))
        {
            Debug.Log("In add pool for " + name);
            _projectilePools.Add(name,new ObjectPool<Projectile>(projectile, size, parent, name));
        }
        return;
    }

    public void RemovePool(string name)
    {
        if (_projectilePools.TryGetValue(name, out ObjectPool<Projectile> _))
            {
               _projectilePools.Remove(name);
            }
        return;
    }

    public Projectile GetObject(string type)
    {
        Projectile obj = null;

        if (_projectilePools.TryGetValue(type, out ObjectPool<Projectile> value))
        {
            obj = value.GetObject();
        }
      

        return obj;
    }

    public void DeactivateObjects(string type)
    {
        if (_projectilePools.TryGetValue(type, out ObjectPool<Projectile> value))
        {
           value.DeactivateAll();
            Debug.Log("In Deactivate objects of type " + type);
        }
    }
}
