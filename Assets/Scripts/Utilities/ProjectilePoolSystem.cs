using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePoolSystem
{
    private Dictionary<string, ObjectPool<Projectile>> _enemyPools;

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
        _enemyPools = new Dictionary<string, ObjectPool<Projectile>>();
    }

    public void AddPool(int size, string name, Projectile projectile, Transform parent)
    {
        if (projectile != null && !_enemyPools.ContainsKey(name))
        {
            _enemyPools.Add(name, new ObjectPool<Projectile>(projectile, size, parent, name));
        }
        return;
    }

    public void RemovePool(string name)
    {
        if (_enemyPools.TryGetValue(name, out ObjectPool<Projectile> _))
        {
            _enemyPools.Remove(name);
        }
        return;
    }

    public Projectile GetObject(string type)
    {
        Projectile obj = null;

        if (_enemyPools.TryGetValue(type, out ObjectPool<Projectile> value))
        {
            obj = value.GetObject();
            Debug.Log("In get projectile obj " + obj);
        }

        return obj;
    }

    public void DeactivateObjects(string type)
    {
        if (_enemyPools.TryGetValue(type, out ObjectPool<Projectile> value))
        {
            value.DeactivateAll();
            Debug.Log("In Deactivate objects of type " + type);
        }
    }
}
