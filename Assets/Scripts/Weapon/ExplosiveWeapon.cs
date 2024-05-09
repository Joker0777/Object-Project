using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/ExplosiveWeapon")]
public class ExplosiveWeapon : Weapon
{
    private string _targetTag;

    public override void ShootWeapon(Vector2 position, Vector2 direction, Quaternion rotation, string targetTag, Transform parent
        )
    {   
        _targetTag = targetTag;
        NukeExplode(position);
    }

    private void NukeExplode(Vector2 position)
    {
        //     Collider2D[] collidersInRange = Physics2D.OverlapCircleAll(position, explosionRadius);

        //   foreach (Collider2D collider in collidersInRange)
        // {
        // if (collider.gameObject.CompareTag(_targetTag))
        //     {
        //        Destroy(collider.gameObject);
        //    }
        // }
        Debug.Log("In Nuke Explode");
        GameManager.Instance.DestoyAllEnemyUnits();
    }
    
}
