using System.Collections;
using System.Collections.Generic;
using UnityEngine;



class AdsUI : MonoBehaviour
{
    [SerializeField] private Button acceptButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private int extraTime;
    [SerializeField] private Timer timer;
        
    void AddTime()
    {
        timer.TimeRemeining += extraTime;
    }
        

}

