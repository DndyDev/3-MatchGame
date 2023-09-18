using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private int timeRemaining = 30; public int TimeRemeining { get { return timeRemaining; } set { timeRemaining = value; } }

    void Update()
    {
        if(timeRemaining > 0)
        {
            timeRemaining -= (int)Time.deltaTime;
        }

    }
}
