using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] private float timerMax = 30; public float TimerMax { get { return timerMax; } set { timerMax = value; } }
    [SerializeField] private TextMeshProUGUI timerTMP;
    [SerializeField] private Slider timerBar;
    [SerializeField] private TimeoutWindow timeout;
    [SerializeField] private ScoreCounter score;

    private float timeRemaining = 0; public float TimeRemaining { get { return timeRemaining; } set { timeRemaining = value; } }


    private void Start()
    {
        timeRemaining = timerMax;
    }

    void Update()
    {
        if(timeRemaining > 0)
        {
            UpdateTimerUI(timeRemaining -= Time.deltaTime);

        }
        else
        {
            timeout.UpdateBestScore();
            timeout.UpdateScore(score.Score);
            timeout.OpenWindow();
        }

    }

    void UpdateTimerUI(float time)
    {
        timerTMP.text = time.ToString("0") + "c.";
        timerBar.value = time;
    }
    public void Restart()
    {
        timeRemaining = timerMax;
    }
}   
