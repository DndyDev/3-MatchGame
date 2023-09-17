using System.Collections;
using System.Collections.Generic;
using UnityEngine;


class Test: MonoBehaviour
    {
        Vector2 vector = new Vector2(1, 2);
        Vector2 offset = new Vector2(0, -1);

    private void Start()
    {
        Debug.Log(vector + offset);
    }
}
