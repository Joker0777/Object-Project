using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{   

    protected float _projectileSpeed;
    protected int _projectileDamage;
    protected string _targetTag;

    protected virtual void Update()
    {
        transform.Translate(Vector3.up * _projectileSpeed * Time.deltaTime);
    }
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {     
        if (collision.gameObject.CompareTag(_targetTag))
        {
            collision.collider.attachedRigidbody.GetComponent<IDamagable>().DamageTaken(_projectileDamage);
        }
        Destroy(this.gameObject);
    }

 
    public void SetupProjectile(int damage, float projectileSpeed, string targetTag)
    {
        _projectileSpeed = projectileSpeed;
        _projectileDamage = damage;
        _targetTag = targetTag;
    }
}
