using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(menuName = "Event Manager")]
public class EventManager : ScriptableObject
{
    //Health
    public Action<int> OnUnitHealthChanged;

    //Unit 
    public Action <UnitType, Vector3>OnUnitDestroyed;

    //UI
    public Action<UIElementType, string> OnUIChange;

    //PickUps
    public Action<PickUp> OnPickupSpawned;

    //Asteroids
    public Action<float, Asteroid> OnAstreroidSplitEvent;

    //Player Input
    public Action<bool> IsThrusting;

    //ParticleSystems
    public Action<string, Vector2, float> OnPlayParticleEffect;
    public Action<string, Vector2> OnWeaponHitParticleEffect;
    public Action<string, Vector2> OnExpolsiveWeaponDetonatedParticleEffect;
    public Action<string, Vector2> OnPickupAddedParticleEffect;

}
