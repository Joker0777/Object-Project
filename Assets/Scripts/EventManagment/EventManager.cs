using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Event Manager")]
public class EventManager : ScriptableObject
{
    public HealthChangeEvents _healthChangeEvents = new HealthChangeEvents();
    public UnitDieEvents _unitDieEvents = new UnitDieEvents();
    public ScoreChangeEvents _scoreChangeEvents = new ScoreChangeEvents();

    public HealthChangeEvents HealthChangeEvents { get { return _healthChangeEvents; } }
    public UnitDieEvents UnitDieEvents { get { return _unitDieEvents; } }
    public ScoreChangeEvents ScoreChangeEvents { get { return _scoreChangeEvents; } }



}
