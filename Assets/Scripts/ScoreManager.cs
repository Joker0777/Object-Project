using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private int score;
    [SerializeField] private int highScore;




    private void Awake()
    {
        highScore = PlayerPrefs.GetInt(highScore.ToString(), highScore);
    }
    public void IncreaseScore()
    {
        score++;
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
