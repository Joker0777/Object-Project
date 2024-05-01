using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/BalisticWeapon")]
public class BalisticWeapon : Weapon
{
    [SerializeField] protected Projectile _projectilePrefab;
    [SerializeField] protected float _weaponSpeed;
    [SerializeField] protected int _weaponDamage;
    [SerializeField] protected string _targetTag;
    [SerializeField] protected int _poolSize = 10;
    [SerializeField] protected string _projectilePoolTag;

    [Range(0f, 1f)]
    [SerializeField] protected float _accuracy;
    private float _maxAngle = 45f;

    protected Projectile _nextProjectile;


    public override void ShootWeapon(Vector2 position, Vector2 direction, Quaternion rotation, string targetTag, Unit userUnit)
    {
        _nextProjectile = ProjectilePoolSystem.Instance.GetObject(_projectilePoolTag);
        Debug.Log("In Shoot Weapon. next projectile " + _nextProjectile);

         if (_nextProjectile == null)
         {

            ProjectilePoolSystem.Instance.AddPool(_poolSize, _projectilePoolTag, _projectilePrefab, userUnit.transform);
            _nextProjectile = ProjectilePoolSystem.Instance.GetObject(_projectilePoolTag);
            Debug.Log("In Shoot Weapon after next projectile was null. next projectile " + _nextProjectile);
        }

         _nextProjectile.transform.position = position;
         _nextProjectile.transform.rotation = SetTrajectoryAngle(direction, rotation);

        //_nextProjectile = Instantiate(_projectilePrefab, position, rotation);

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
