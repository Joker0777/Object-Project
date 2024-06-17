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
    [SerializeField] protected int _poolSize = 10;

    private float _maxAngle = 45f;

    public override void ShootWeapon(Vector2 position, Vector2 direction, Quaternion rotation, string targetTag, Transform parent)
    {

        Projectile nextProjectile = ObjectPoolSystem<Projectile>.Instance.GetObject(_weaponObjectPoolTag);

        if (nextProjectile == null)
        {
            ObjectPoolSystem<Projectile>.Instance.AddPool(_poolSize, _weaponObjectPoolTag, _projectilePrefab, parent);
            nextProjectile = ObjectPoolSystem<Projectile>.Instance.GetObject(_weaponObjectPoolTag);
        }

 
        if (nextProjectile != null)
        {
            nextProjectile.transform.position = position;
            nextProjectile.transform.rotation = SetTrajectoryAngle(direction, rotation);
            nextProjectile.SetupProjectile(_weaponDamage, _weaponSpeed, targetTag);
        }
    }

    private Quaternion SetTrajectoryAngle(Vector2 direction, Quaternion rotation)
    {
        float accuracyRange = Mathf.Lerp(_maxAngle, 0, _accuracy);
        float randomAngle = UnityEngine.Random.Range(-accuracyRange, accuracyRange);
        Quaternion projectileRotation = Quaternion.AngleAxis(randomAngle, Vector3.forward);
        return rotation * projectileRotation;
    }
}
