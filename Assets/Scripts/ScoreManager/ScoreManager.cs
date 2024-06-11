using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{

    [SerializeField] private int highScore;

    [SerializeField] private EventManager eventManager;

    [SerializeField] private List<ElementScore> _elementScores;

    private int score;


    private void Awake()
    {
        highScore = PlayerPrefs.GetInt(highScore.ToString(), highScore);
    }

    public void UpdateScore(string element)
    {
        foreach (var elementScore in _elementScores) 
        { 
            if(elementScore.ScoreElementTypeTag == element)
            {
                score += elementScore.ScoreAmount;
                return;
            }
        }
    }

    public void RegisterHighScore()
    {
        if(score> highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt(highScore.ToString(), highScore);
        }
    }

    
}
