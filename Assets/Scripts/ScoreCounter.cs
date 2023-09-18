using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class ScoreCounter : MonoBehaviour
{
     private int score { set; get; } = 0;
     private int bestScore { set; get; } = 0;


    public void addScore(int matchesNodesCount)
    {
        if (matchesNodesCount == 3)
        {
            this.score += matchesNodesCount;
        }
        else if(matchesNodesCount > 3)
        {
            this.score += matchesNodesCount * (matchesNodesCount % 3);
        }

    }

    public void UpdateBestScore()
    {
        if (bestScore < score) bestScore = score;
    }
}

