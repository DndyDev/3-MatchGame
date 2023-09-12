using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Playground : MonoBehaviour
{
    [SerializeField] private  int width = 7;
    [SerializeField] private  int height = 7;
    [SerializeField] private float spacing;

    [SerializeField] private float speed = 5.5f; 
    [SerializeField] private float destroyTimeout = .5f; // пауза в секундах, перед тем как уничтожить совпадения
    [SerializeField] private LayerMask pieceLayer; 
    [SerializeField] private Sprite[] sprites; // набор цветов

    [SerializeField] private PieceNode piece;
    [SerializeField] private ScoreCounter counter;

    private ObjectPool<PieceNode> poolOfPieces;
    private PieceNode[] nodeArray;
    private Vector3[,] position;
    private PieceNode current, last;
    private Vector3 currentPos, lastPos;
    private List<PieceNode> lines;
    private bool isLines, isMove, isMode;
    private float timeout;

    void Start()
    {
        poolOfPieces = new ObjectPool<PieceNode>();
        CreatePlayground();
    }

    private void Update()
    {
        
    }

    private void InitiatePlayground()
    {
        position = new Vector3[width, height];
        nodeArray = new PieceNode[width * height];


        int i = 0;
        int id = -1;
        int step = 0;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int colorId = Random.Range(0, sprites.Length);
                if (id != colorId) id = colorId; else step++;
                if (step > 2)
                {
                    step = 0;
                    id = (id + 1 < sprites.Length - 1) ? id + 1 : id - 1;
                    id = id + 1;
                }

               poolOfPieces.GetObject().Ready = false;
               poolOfPieces.GetObject().X = x;
               poolOfPieces.GetObject().y = y;
                grid[x, y].id = id;
                grid[x, y].sprite.color = color[id];
                grid[x, y].gameObject.SetActive(true);
                grid[x, y].highlight.SetActive(false);
                position[x, y] = grid[x, y].transform.position;
                nodeArray[i] = grid[x, y];
                i++;
            }
        }

        current = null;
        last = null;
    }
    private void MovePieces()
    {

    }
    private void UpdatePlayground()
    {

    }
    private void CreatePlayground()
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
                poolOfPieces.ReturnObject(Instantiate(piece, new Vector2(
                    transform.position.x + posX * piece.transform.localScale.x,
                    transform.position.y + posY * piece.transform.localScale.y
                    ),Quaternion.identity, parentTransform));
                poolOfPieces.GetObject().name = "Node-" + (y + x);
            }
            posY -= spacing;
            posX = tempX;
        }

    }
}
