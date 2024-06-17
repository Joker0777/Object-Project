using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[CreateAssetMenu(menuName = "Weapons/ExplosiveWeapon")]
public class ExplosiveWeapon : Weapon
{
    private string _targetTag;

    [SerializeField] private LayerMask _damageLayer;
    [SerializeField] private float _explosionRadius = 5f;
    [SerializeField] private int _explosiveDamage = 100;
    [SerializeField] private float _explosionForce = 100f;
    [SerializeField] EventManager _eventManager;

  
    public override void ShootWeapon(Vector2 position, Vector2 direction, Quaternion rotation, string targetTag, Transform parent)
    {   
        _targetTag = targetTag;
        ExplodeWeapon(position);
    }


    private void ExplodeWeapon(Vector2 position)
    {
        Collider2D[] collidersExplode = Physics2D.OverlapCircleAll(position, _explosionRadius);

        foreach (var collider in collidersExplode)
        {
            if (IsTargetTag(collider) || IsInDamageLayers(collider))
            {
                collider.attachedRigidbody?.GetComponent<IDamagable>()?.DamageTaken(_explosiveDamage);
            }
        }
        _eventManager.OnPlayParticleEffect("Explosion", position, _explosionRadius);
        
    }

    private bool IsTargetTag(Collider2D collider)
    {
        return collider.CompareTag(_targetTag);
    }

    private bool IsInDamageLayers(Collider2D collider)
    {
        return (_damageLayer.value & (1 << collider.gameObject.layer)) != 0;
    }

}
    

