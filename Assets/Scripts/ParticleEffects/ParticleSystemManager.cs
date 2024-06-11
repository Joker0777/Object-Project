using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class ParticleSystemManager : MonoBehaviour
{
  //  public static ParticleSystemManager Instance;

    [SerializeField] private int particleSystemPoolSize;
    [SerializeField] private Transform particleSystemParent;

    [SerializeField] private ParticleEffect[] _particleEffectPrefabs;
    [SerializeField] private EventManager _eventManager;

   // private Dictionary<string, ObjectPool<ParticleEffect>> _particleEffectPools;

    private void Awake()
    {
        
           // if (Instance == null)
           // {
           //     Instance = this;
           //     DontDestroyOnLoad(gameObject);
          //  }
          //  else
         //   {
         //      Destroy(gameObject);
         //  }
        

       // _particleEffectPools = new Dictionary<string, ObjectPool<ParticleEffect>>();


    }
    void Start()
    {
        InitializeParticlePools();
    }

    private void OnEnable()
    {
        _eventManager.OnPlayParticleEffect += GetParticleEffect;
    }

    private void OnDisable()
    {
        _eventManager.OnPlayParticleEffect -= GetParticleEffect;
    }

    public void GetParticleEffect(string particleEffectTag, Vector2 position, float scale)
    {
        //if(_particleEffectPools.TryGetValue(particleEffectTag, out var pool))
        // {
        //   pool.GetObject().PlayParticleEffect(position,scale);
        //}
        ParticleEffect particleEffect = ObjectPoolSystem<ParticleEffect>.Instance.GetObject(particleEffectTag);
        particleEffect?.PlayParticleEffect(position, scale);
     
    }

    private void InitializeParticlePools()
    {
        foreach (var pool in _particleEffectPrefabs)
        {
           // _particleEffectPools.Add(pool.ParticleSystemTag,
              //  new ObjectPool<ParticleEffect>(pool, particleSystemPoolSize, particleSystemParent, pool.ParticleSystemTag));
            ObjectPoolSystem<ParticleEffect>.Instance.AddPool(particleSystemPoolSize, pool.ParticleSystemTag, pool, particleSystemParent);
        }
    }
}
