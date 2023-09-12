using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class ScoreCounter : MonoBehaviour
{
    private int score { set; get; } = 0;

        public void addScore(int score)
        {
            this.score += score;
        }
    }

