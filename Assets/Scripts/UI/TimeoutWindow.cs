using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;



class TimeoutWindow : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI score;
    [SerializeField] private TextMeshProUGUI bestScore;
    [SerializeField] private int extraTime;
    [SerializeField] private Timer timer;
    [SerializeField] private RewardOnClick reward;

    private bool resume = true;

    public void AddTime()
    {
        timer.TimeRemaining += extraTime;
        CloseWindow();
    }

    public void ShowAds()
    {
        if (resume)
        {
            reward.ShowAd();
            resume = false;
        }
    }

    public void OpenWindow()
    {
        gameObject.SetActive(true);
    }

    public void CloseWindow()
    {
        gameObject.SetActive(false);
    }
     public void UpdateScore(int score)
     {
        this.score.text = score.ToString();
     }
    public void UpdateBestScore()
    {
        this.bestScore.text = PlayerPrefs.GetInt("bestScore").ToString();
    }





}

