using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(menuName = "Event Manager")]
public class EventManager : ScriptableObject
{
    private static EventManager _instance;

    public static EventManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.Load<EventManager>("EventManager");
                if (_instance == null)
                {
                    Debug.LogError("EventManager instance not found. Make sure there is an EventManager asset in a Resources folder.");
                }
            }
            return _instance;
        }
    }

    // Health
    public Action<int> OnUnitHealthChanged;

    // Unit
    public Action<UnitType, Vector3> OnUnitDestroyed;

    // UI
    public Action<UIElementType, string> OnUIChange;

    // PickUps
    public Action<PickUp> OnPickupSpawned;

    // Asteroids
    public Action<float, Asteroid> OnAstreroidSplitEvent;

    // Player Input
    public Action<bool> IsThrusting;

    // ParticleSystems
    public Action<string, Vector2, float> OnPlayParticleEffect;

    //Audio
    public Action<string, Vector2> OnPlaySoundEffect;

    //PlayerRespawn
    public Action<Unit> OnPlayerRespawn;

    //Score
    public Action<string> OnScoreIncrease;
    public Action<string> OnGetHighScore;

    //Scene/GameEnd
    public Action OnGameSceneEnd;

}
