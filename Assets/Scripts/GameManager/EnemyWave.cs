using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EnemyWave")]
public class EnemyWave : ScriptableObject
{
    [SerializeField] private List<Unit> _units = new List<Unit>();
    [SerializeField] private int _spawnInterval;

    public float SpawnInterval =>_spawnInterval;

    public List<Unit> GetEnemyUnits()
    {
        return new List<Unit>(_units);
    }  
}
