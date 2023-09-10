using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playground : MonoBehaviour
{
    [SerializeField] private  int width = 7;
    [SerializeField] private  int height = 7;
    [SerializeField] private float spacing;


    [SerializeField] private GameObject tile;
    //[SerializeField] private Timer timer;

    private GameObject[,] gridTiles;

    void Start()
    {
        gridTiles = new GameObject[width, height];
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
                gridTiles[x,y] =  Instantiate(tile, new Vector2(
                    transform.position.x + posX * tile.transform.localScale.x ,
                    transform.position.y + posY * tile.transform.localScale.y
                    ),Quaternion.identity, parentTransform);
            }
            posY -= spacing;
            posX = tempX;
        }

    }
}
