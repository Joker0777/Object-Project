using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/ProjectileWeapon")]
public class ProjectileWeapon : Weapon
{

    [Range(0f, 1f)]
    [SerializeField] protected float _accuracy;
    [SerializeField] protected float _weaponSpeed;
    [SerializeField] protected int _weaponDamage;
    [SerializeField] protected string _targetTag;

    [SerializeField] protected Projectile _projectilePrefab;

    [Header("Object Pool Setup")]
    [SerializeField] protected int _poolSize = 10;
   // [SerializeField] protected string _projectilePoolTag;

 

    private float _maxAngle = 45f;

    protected Projectile _nextProjectile;






    public override void ShootWeapon(Vector2 position, Vector2 direction, Quaternion rotation, string targetTag, Transform parent)
    {
        _nextProjectile = ObjectPoolSystem<Projectile>.Instance.GetObject(_weaponType);

         if (_nextProjectile == null)
         {
            ObjectPoolSystem<Projectile>.Instance.AddPool(_poolSize, _weaponType, _projectilePrefab, parent);
            _nextProjectile = ObjectPoolSystem<Projectile>.Instance.GetObject(_weaponType);
        }

         _nextProjectile.transform.position = position;
         _nextProjectile.transform.rotation = SetTrajectoryAngle(direction, rotation);

        _nextProjectile.SetupProjectile(_weaponDamage, _weaponSpeed, targetTag);
    }

    private Quaternion SetTrajectoryAngle(Vector2 direction, Quaternion rotation)
    {
        float accuracyRange = Mathf.Lerp(_maxAngle, 0, _accuracy);
        float randomAngle = UnityEngine.Random.Range(-accuracyRange, accuracyRange);

        Quaternion projectileRotation = Quaternion.AngleAxis(randomAngle, Vector3.forward);

        return rotation * projectileRotation;
    }
}
