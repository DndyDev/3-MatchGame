using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

class ScoreCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private int matchConst = 3;
    
    private int score = 0; public int Score { get { return score; } set { score = value; } } 
    private int bestScore;

    private void Awake()
    {
        bestScore = PlayerPrefs.GetInt("bestScore", 0); 
    }

    public void addScore(int matchesNodesCount)
    {
        if (matchesNodesCount == matchConst)
        {
            this.score += matchesNodesCount;
        }
        else if (matchesNodesCount > matchConst)
        {
            this.score += matchesNodesCount * (matchesNodesCount - matchConst + 1);
        }

        UpdateScoreUI(this.score);
    }

    public void UpdateBestScore(int score)
    {
        
        if (bestScore < score) {
            bestScore = score;
            PlayerPrefs.SetInt("bestScore", bestScore);
        };
    }

    public void UpdateScoreUI(int score)
    {
        scoreText.text = score.ToString();
        UpdateBestScore(score);
    }

    private void UpdateScore(int score)
    {
        this.score = score;
        UpdateScoreUI(score);
    }

    public void Restart()
    {
        UpdateScore(0);
    }
}

