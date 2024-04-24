using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Weapons/SelfDestructWeapon")]
public class SelfDestructWeapon : Weapon
{

    [SerializeField] protected int _weaponDamage = 70;
    [SerializeField] protected float explosionRadius = 5f;
    
    private Unit _userUnit;
    private string _targetTag;

     public override void ShootWeapon(Vector2 position, Vector2 direction, Quaternion rotation, string targetTag, Unit userUnit)
 
    {     
       _userUnit = userUnit;
       _targetTag = targetTag; 


       SelfDestruct(position);
    }

    private void SelfDestruct(Vector2 position)
    {
            Collider2D[] collidersInRange = Physics2D.OverlapCircleAll(position, explosionRadius);

            foreach (Collider2D collider in collidersInRange)
            {
                if (collider.gameObject.CompareTag(_userUnit.gameObject.tag) || collider.gameObject.CompareTag(_targetTag))
                {
                    Debug.Log("Game object tag " + _userUnit.gameObject.tag);
                    collider.attachedRigidbody.GetComponent<IDamagable>().DamageTaken(_weaponDamage);
                }
            }
        Destroy(_userUnit.gameObject);
    }
}
