using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScoreElement")]
public class ElementScore : ScriptableObject
{
    [SerializeField] private int _scoreAmount;
    [SerializeField] private string _elementType;

    public int ScoreAmount {  get { return _scoreAmount; } }
    public string ScoreElementTypeTag { get { return _elementType; } }
}
