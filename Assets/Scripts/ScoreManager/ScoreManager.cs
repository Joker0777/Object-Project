using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{

    [SerializeField] private int highScore;



    [SerializeField] private List<ElementScore> _elementScores;

    private int score;
    private EventManager eventManager;


    private void Awake()
    {
        highScore = PlayerPrefs.GetInt(highScore.ToString(), highScore);
        eventManager = EventManager.Instance;
    }

    private void Start()
    {
        eventManager.OnUIChange?.Invoke(UIElementType.Score, "0");
        Debug.Log("High score is on start " + highScore.ToString());
    }


    private void OnEnable()
    {
        eventManager.OnScoreIncrease += UpdateScore;
        eventManager.OnGameSceneEnd += RegisterHighScore;
    }

    private void OnDisable()
    {
        eventManager.OnScoreIncrease -= UpdateScore;
        eventManager.OnGameSceneEnd -= RegisterHighScore;
    }

    public void UpdateScore(string element)
    {
        foreach (var elementScore in _elementScores) 
        { 
            if(elementScore.ScoreElementTypeTag == element)
            {
                score += elementScore.ScoreAmount;
                eventManager.OnUIChange?.Invoke(UIElementType.Score, score.ToString());
            }
        }
    }

    public void RegisterHighScore()
    {
        if(score> highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt(highScore.ToString(), highScore);
            PlayerPrefs.Save();
            eventManager.OnGetHighScore?.Invoke(highScore.ToString());
            Debug.Log("High score is " + highScore.ToString());
        }
    }

    
}
