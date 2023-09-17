using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Pool;
using Pool;

public class Playground : MonoBehaviour
{
    [SerializeField] private  int width = 7;
    [SerializeField] private  int height = 7;
    [SerializeField] private float spacing;

    [SerializeField] private float speed = 5.5f; 
    [SerializeField] private float destroyTimeout = .5f; // пауза в секундах, перед тем как уничтожить совпадени€
    [SerializeField] private LayerMask pieceLayer; 
    [SerializeField] private Sprite[] sprites; // набор цветов

    [SerializeField] private PieceNode piece;
    [SerializeField] private ScoreCounter counter;

    //private Match3Pool<PieceNode> poolOfPieces; // хранит фишки
    private PieceNode tempPiece;
    private float timeout;
    private Vector2Int coordinates;
    private List<Vector2> matchesCoordinates;
    private Dictionary<Vector2,PieceNode> grid; // хранит текущее состо€ние сетки

    private PieceNode[] nodeArray;
    private Vector3[,] position;
    private PieceNode current, last;
    private Vector3 currentPos, lastPos;
    private List<PieceNode> lines;
    private bool isLines, isMove, isMode;

    void Start()
    {
        //poolOfPieces = new Match3Pool<PieceNode>();
        grid = new Dictionary<Vector2, PieceNode>();
        CreatePlayground();
        InitiatePlayground();
    }

    private void Update()
    {
        MarkMatchesPieces();
        DeleteMatchesPieces();

    }


    private void CreatePlayground()
    {
        int nodeCounter = 1;
        Transform parentTransform = gameObject.transform;

        float posX = (width + 1) * -spacing / 2f;
        float posY = (height - 1) * spacing / 2f;   

        float tempX = posX;


        for (int y = 0; y < width; y++)
        {
            for (int x = 0; x < height; x++)
            {
                coordinates = new Vector2Int(x, y);
                posX += spacing;
                tempPiece = Instantiate(piece, new Vector2(
                    transform.position.x + posX * piece.transform.localScale.x,
                    transform.position.y + posY * piece.transform.localScale.y
                    ), Quaternion.identity, parentTransform);

                tempPiece.name = "Node-" + (nodeCounter);
                nodeCounter++;
                //poolOfPieces.ReturnObject(tempPiece);
            }
            posY -= spacing;
            posX = tempX;
        }

    }

    private void InitiatePlayground()
    {
        position = new Vector3[width, height];
        nodeArray = new PieceNode[width * height];

        int id = -1;
        int step = 0;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                coordinates = new Vector2Int(x, y);
                int spriteId = Random.Range(0, sprites.Length);
                if (id != spriteId) id = spriteId; else step++;
                if (step > 2)
                {
                    step = 0;
                    id = (id + 1 < sprites.Length - 1) ? id + 1 : id - 1;
                    id = id + 1;
                }

                //tempPiece = poolOfPieces.GetObject();

                tempPiece.Ready = false;
                tempPiece.X = x;
                tempPiece.Y = y;
                tempPiece.Id = id;
                tempPiece.Renderer.sprite = sprites[id];
                grid.Add(coordinates, tempPiece);
                //position[x, y] = tempPiece.transform.position;
                //nodeArray[i] = tempPiece;
            }
        }

        current = null;
        last = null;
    }

    private void MarkMatchesPieces()
    {
        matchesCoordinates = new List<Vector2>();
        timeout += Time.deltaTime;

        if (timeout > destroyTimeout)
        {
            for (int x = 0; x < width; x++)
            {
              for (int y = 0; y < height; y++)
              {
                    coordinates = new Vector2Int(x, y);
                FindMatches(coordinates, new Vector2Int(1, 0), 3);
                FindMatches(coordinates, new Vector2Int(0, 1), 3);
              }
            }
            isMove = true;    
        }
    }

    /// <summary>
    /// »щет совпадени€ на заданное смещение ограничева€сь заданным числом 
    /// </summary>
    /// <param name="coordinates">Ќачальна€ точка</param>
    /// <param name="offsets">—мещение</param>
    /// <param name="matchesCount"> оличество совпадений</param>
    void FindMatches(Vector2Int coordinates, Vector2Int offsets, int matchesCount)
    {
        int currentId = grid[coordinates].Id;
        for (Vector2Int temp = coordinates + offsets;
            temp.x >= 0 && temp.x < width && temp.y >= 0 && temp.y < height
            && grid[temp].Id == currentId;
            temp += offsets)
        {
            if (grid[coordinates].Visited == false)
            {
                matchesCoordinates.Add(coordinates);
                grid[coordinates].Visited = true;
            }
            else
            {
                break;
            }
        }
        if (matchesCoordinates.Count < 3)
        {
            matchesCoordinates.Clear();
        }
    }

    private void DeleteMatchesPieces()
    {
        // ≈сли есть 3 или более совпадени€ по горизонтали или вертикали, удалите узлы
        if (matchesCoordinates.Count > 0)
        {
            foreach (Vector2 vector in matchesCoordinates)
            {
                //poolOfPieces.ReturnObject(grid[vector]);
                grid[vector] = null;
            }
            // вынести в отдельный метод  update counter
            //counter.addScore(horizontalMatches + verticalMatches);
        } 
    }

    private void MovePieces()
    {

    }
    private void UpdatePlayground()
    {

    }
    

}
