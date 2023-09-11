using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playground : MonoBehaviour
{
    [SerializeField] private  int width = 7;
    [SerializeField] private  int height = 7;
    [SerializeField] private float spacing;


    [SerializeField] private GameObject node;
    //[SerializeField] private Timer timer;

    private GameObject[,] nodes;

    void Start()
    {
        nodes = new GameObject[width, height];
        createPlayground();
    }


    private void createPlayground()
    {
        Transform parentTransform = gameObject.transform;

        float posX = (width + 1) * -spacing / 2f;
        float posY = (height - 1) * spacing / 2f;

        float tempX = posX;


        for (int y = 0; y < width; y++)
        {
            for(int x = 0; x < height; x++)
            {
                posX += spacing;
                nodes[x,y] =  Instantiate(node, new Vector2(
                    transform.position.x + posX * node.transform.localScale.x ,
                    transform.position.y + posY * node.transform.localScale.y
                    ),Quaternion.identity, parentTransform);
                nodes[x, y].name = "Node-" + (y + x);
            }
            posY -= spacing;
            posX = tempX;
        }

    }
}
