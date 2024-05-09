using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldSystem : UnitSystems
{
    [SerializeField]
    private float _asteroidDamage = 25f;

    [SerializeField]
    private float _maxTrasparency = .8f;

    private float _maxShieldValue = 100f;
    private float _currentShieldValue;


    private SpriteRenderer _shieldSprite;
    private Color _shieldColor;

    
    protected override void Awake()
    {
        base.Awake();
        _shieldSprite = GetComponent<SpriteRenderer>();
    }

    protected override void Start()
    {
        base.Start();
        _shieldColor = _shieldSprite.color;
        _currentShieldValue = 0f;
        SetTransparencyColor(0f, _shieldSprite.color);
    }

    private void DamageShield()
    {
        _currentShieldValue -= _asteroidDamage;

        StartCoroutine(FadeShield());
        
        if (_currentShieldValue <= 0) 
        { 
            this.gameObject.SetActive(false);
        }    
    }

    IEnumerator FadeShield()
    {
        float startingTransparency = _shieldSprite.color.a;
        float endingTransparency = Mathf.Lerp(0, _maxTrasparency, _currentShieldValue/_maxShieldValue);

        Color startColor = _shieldSprite.color;
        Color finalColor = Color.red;
        Color endColor = Color.Lerp(startColor, finalColor, _currentShieldValue / _maxShieldValue);

        float currentTimer = 0;
        float duration = 2f;


        while(currentTimer < duration)
        {
            float transparency = Mathf.Lerp(startingTransparency, endingTransparency, currentTimer / duration);
            Color currentColor = Color.Lerp(startColor, endColor, currentTimer / duration);
            
            SetTransparencyColor(transparency, currentColor);

            currentTimer += Time.deltaTime;
            yield return null;
        }

        SetTransparencyColor(endingTransparency, endColor);
    }

    private void SetTransparencyColor(float transparency, Color color)
    {
        Color newColor = color;
        newColor.a = transparency;
        _shieldSprite.color = newColor;
        
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Asteroid")
        {
            DamageShield();
        }
    }


}
