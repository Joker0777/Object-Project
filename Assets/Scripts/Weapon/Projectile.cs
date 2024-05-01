using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float _projectileTimerLength = 6f;

    protected float _projectileSpeed;
    protected int _projectileDamage;
    protected string _targetTag;

    private Timer _projectileTimer;
    private bool _fired;

    protected virtual void Update()
    {
        transform.Translate(Vector3.up * _projectileSpeed * Time.deltaTime);

        if (_fired)
        {
            if (!_projectileTimer.IsRunning)
            {
                _fired = false;
                this.gameObject.SetActive(false);
            }
          
        }    
    }
    private void Awake()
    {
        _projectileTimer = new Timer(this, _projectileTimerLength);
    }

    //  private void Start()
    //  {
    //      SetUpTimer();
    // }

    //  private void OnEnable()
    //  {
    //      SetUpTimer();      
    // }

    private void SetUpTimer()
    {

    }
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {     
        if (collision.gameObject.CompareTag(_targetTag))
        {
            collision.collider.attachedRigidbody.GetComponent<IDamagable>().DamageTaken(_projectileDamage);
        }
        this.gameObject.SetActive(false);
    }

 
    public void SetupProjectile(int damage, float projectileSpeed, string targetTag)
    {
        _projectileSpeed = projectileSpeed;
        _projectileDamage = damage;
        _targetTag = targetTag;

        _projectileTimer.TimerDuration = _projectileTimerLength;
        _projectileTimer.StartTimer();
        _fired = true;
        Debug.Log("In setup projectile.");
    }
}
