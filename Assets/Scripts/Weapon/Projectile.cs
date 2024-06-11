using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] EventManager eventManager;
    [SerializeField] float _projectileTimerLength = 6f;
    [SerializeField] float _particleEffectScale = 1f;

    protected float _projectileSpeed;
    protected int _projectileDamage;
    protected string _targetTag;

    private Timer _projectileTimer;
    private bool _fired;

    private Rigidbody2D rb;


    private void Awake()
    {
        _projectileTimer = new Timer(_projectileTimerLength);

        rb = GetComponent<Rigidbody2D>();
    }


    protected virtual void Update()
    {

        if (_fired)
        {
            _projectileTimer.UpdateTimerBasic(Time.deltaTime);
            
            if (!_projectileTimer.IsRunningBasic())
            {
                _fired = false;
                this.gameObject.SetActive(false);
            }
          
        }    
    }

    private void ShootProjectile()
    {
        rb.AddForce(transform.up * _projectileSpeed);
    }


    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        Vector3 weaponHit = collision.contacts[0].point;
        
        if (collision.gameObject.CompareTag(_targetTag) || collision.gameObject.CompareTag("Asteroid"))
        {
            collision.collider.attachedRigidbody.GetComponent<IDamagable>().DamageTaken(_projectileDamage);

        }


        this.gameObject.SetActive(false);

        eventManager.OnPlayParticleEffect?.Invoke("Projectile", (Vector2)weaponHit,_particleEffectScale);

    }

 
    public void SetupProjectile(int damage, float projectileSpeed, string targetTag)
    {
        _projectileSpeed = projectileSpeed;
        _projectileDamage = damage;
        _targetTag = targetTag;

        _projectileTimer.TimerDuration = _projectileTimerLength;
        _projectileTimer.StartTimerBasic();

        ShootProjectile();

        _fired = true;
    }
}
