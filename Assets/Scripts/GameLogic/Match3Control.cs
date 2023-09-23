using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pool;

public class Match3Control : MonoBehaviour
{

	[SerializeField] private float speed = 400; // скорость движения объектов
	[SerializeField] private float destroyTimeout = .5f; // пауза в секундах, перед тем как уничтожить совпадения
	[SerializeField] private LayerMask layerMask; // маска узла (префаба)
	[SerializeField] private Sprite[] sprites; // набор цветов/id
	[SerializeField] private int gridWidth = 7; // ширина игрового поля
	[SerializeField] private int gridHeight = 10; // высота игрового поля
	[SerializeField] private Match3Node sampleObject; // образец узла (префаб)
	[SerializeField] private float spacing = 1.3f; // размер узла (ширина и высота)
	[SerializeField] private ScoreCounter score;

	private Match3Pool<Match3Node> nodePool;
	private Match3Node[,] grid;
	private Match3Node[] nodeArray;
	private Vector3[,] position;
	private Match3Node current, last;
		public Match3Node Last { get { return last; } set { last = value; }}
		public Match3Node Current { get { return current; } set { current = value; }}

	private Vector3 currentPos, lastPos;
	public Vector3 LastPos { get { return lastPos; } set { lastPos = value; } }
	public Vector3 CurrentPos { get { return currentPos; } set { currentPos = value; } }
	private List<Match3Node> matches;
	private bool isLines, isMove, isMode; 
		public bool IsMode { get { return isMode; } set {isMode = value; } }
		public bool IsLines { get { return isLines; } set {isLines = value; } }
		public bool IsMove { get { return isMove; } set {isMove = value; } }
	private float timeout;

	void Start()
	{
		nodePool = new Match3Pool<Match3Node>();
		grid = Create2DGrid<Match3Node>(sampleObject,gridWidth, gridHeight, spacing);
        InitiatePlayground();
    }

	void Update()
	{
        DestroyMatches();

        MoveNodes();

    }

	public void Restart()
	{
		nodePool.Clear();
		for (int y = 0; y < gridHeight; y++)
		{
			for (int x = 0; x < gridWidth; x++)
			{
				//nodePool.ReturnObject(grid[x, y]);
				grid[x, y] = null;

			}
		}
		InitiatePlayground();
	}
	/// <summary>
	/// Подготовка игрового поля
	/// </summary>
    void InitiatePlayground()
	{
        position = new Vector3[gridWidth, gridHeight];
        nodeArray = new Match3Node[gridWidth * gridHeight];

		int i = 0;
		int id = -1;
		int step = 0;

		for (int y = 0; y < gridHeight; y++)
		{
			for (int x = 0; x < gridWidth; x++)
			{
				int spriteId = Random.Range(0, sprites.Length);
				if (id != spriteId) id = spriteId; else step++;
				if (step > 2)
				{
					step = 0;
					id = (id + 1 < sprites.Length - 1) ? id + 1 : id - 1;
					id = id + 1;
				}

				grid[x, y].ready = false;
				grid[x, y].x = x;
				grid[x, y].y = y;
				grid[x, y].id = id;
				grid[x, y].sprite.sprite = sprites[id];
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

	/// <summary>
	/// Удаление совпадений
	/// </summary>
	void DestroyMatches()
	{
		if (!isLines) return;
		int piecesNumber = 0;
		List<Match3Node> nodes = new List<Match3Node>();
		timeout += Time.deltaTime;

		if (timeout > destroyTimeout)
		{
			if(matches.Count > 0)
			
			for (int i = 0; i < matches.Count; i++)
			{
	
                nodePool.ReturnObject(matches[i]);
				grid[matches[i].x, matches[i].y] = null;

				for (int y = matches[i].y - 1; y >= 0; y--)
				{
					if (grid[matches[i].x, y] != null)
					{
						grid[matches[i].x, y].move = true;
					}
				}
			}

			isMove = true;
			isLines = false;
        }
	}

	/// <summary>
	/// Сдвиг узлов
	/// </summary>
	void MoveNodes() 
	{
		if (!isMove) return;

		for (int y = 0; y < gridHeight; y++)
		{
			for (int x = 0; x < gridWidth; x++)
			{
				if (grid[x, 0] == null)
				{
					bool check = true;

					for (int i = 0; i < gridWidth; i++)
					{
						if (grid[i, 0] == null)
						{
							grid[i, 0] = GetNewNode(position[i, 0]);
						}
					}

					for (int i = 0; i < nodeArray.Length; i++)
					{
						if (!nodeArray[i].gameObject.activeSelf) check = false;
					}

					if (check)
					{
						isMove = false;
						GridUpdate();

						if (IsMatche())
						{
							timeout = 0;
							isLines = true;
						}
						else
						{
							isMode = false;
						}
					}
				}

				if (grid[x, y] != null)
					if (y + 1 < gridHeight && grid[x, y].gameObject.activeSelf && grid[x, y + 1] == null)
					{
						grid[x, y].transform.position = Vector3.MoveTowards(grid[x, y].transform.position, position[x, y + 1], speed * Time.deltaTime);

						if (grid[x, y].transform.position == position[x, y + 1])
						{
							grid[x, y + 1] = grid[x, y];
							grid[x, y] = null;
						}
					}
			}
		}
	}

	/// <summary>
	/// Возвращает новую фишку
	/// </summary>
	/// <param name="pos">позиция фишки</param>
	/// <returns></returns>
	Match3Node GetNewNode(Vector3 pos)
	{
		int j = Random.Range(0, sprites.Length);		
		return nodePool.GetObject(j, sprites[j], pos);
	}

	/// <summary>
	/// Обновление поля 
	/// </summary>
	void GridUpdate()
	{
		for (int y = 0; y < gridHeight; y++)
		{
			for (int x = 0; x < gridWidth; x++)
			{
				RaycastHit2D hit = Physics2D.Raycast(position[x, y], Vector2.zero, Mathf.Infinity, layerMask);

				if (hit.transform != null)
				{
					grid[x, y] = hit.transform.GetComponent<Match3Node>();
					grid[x, y].ready = false;
					grid[x, y].x = x;
					grid[x, y].y = y;
				}
			}
		}
	}

	/// <summary>
	/// Перемещение узла игрока
	/// </summary>
	 public void MoveCurrent()
	{
		current.transform.position = Vector3.MoveTowards(current.transform.position, lastPos, speed * Time.deltaTime);
		last.transform.position = Vector3.MoveTowards(last.transform.position, currentPos, speed * Time.deltaTime);

		if (current.transform.position == lastPos && last.transform.position == currentPos)
		{
			GridUpdate();

			if (isMode && !CheckNearNodes(current) && !CheckNearNodes(last))
			{
				currentPos = position[current.x, current.y];
				lastPos = position[last.x, last.y];
				isMode = false;
				return;
			}
			else
			{
				isMode = false;
			}

			current = null;
			last = null;

			if (IsMatche())
			{
				timeout = 0;
				isLines = true;
			}
		}
	}

	/// <summary>
	/// Проверка есть ли соседы у текущей фишки
	/// </summary>
	/// <param name="node">Фишка</param>
	/// <returns>bool</returns>
	bool CheckNearNodes(Match3Node node)
	{
		if (node.x - 2 >= 0)
			if (grid[node.x - 1, node.y].id == node.id && grid[node.x - 2, node.y].id == node.id) return true;

		if (node.y - 2 >= 0)
			if (grid[node.x, node.y - 1].id == node.id && grid[node.x, node.y - 2].id == node.id) return true;

		if (node.x + 2 < gridWidth)
			if (grid[node.x + 1, node.y].id == node.id && grid[node.x + 2, node.y].id == node.id) return true;

		if (node.y + 2 < gridHeight)
			if (grid[node.x, node.y + 1].id == node.id && grid[node.x, node.y + 2].id == node.id) return true;

		if (node.x - 1 >= 0 && node.x + 1 < gridWidth)
			if (grid[node.x - 1, node.y].id == node.id && grid[node.x + 1, node.y].id == node.id) return true;

		if (node.y - 1 >= 0 && node.y + 1 < gridHeight)
			if (grid[node.x, node.y - 1].id == node.id && grid[node.x, node.y + 1].id == node.id) return true;

		return false;
	}

	/// <summary>
	/// Запрет на перемещение соседних фишек
	/// </summary>
	/// <param name="node">фишка</param>
	/// <param name="value">флаг</param>
	public void SetNode(Match3Node node, bool value) 
	{
		if (node == null) return;

		if (node.x - 1 >= 0) grid[node.x - 1, node.y].ready = value;
		if (node.y - 1 >= 0) grid[node.x, node.y - 1].ready = value;
		if (node.x + 1 < gridWidth) grid[node.x + 1, node.y].ready = value;
		if (node.y + 1 < gridHeight) grid[node.x, node.y + 1].ready = value;
	}


	/// <summary>
	/// Проверка совпаденений по горизонтали и вертикали
	/// </summary>
	/// <returns></returns>
	bool IsMatche()
	{
		int j = -1;

		matches = new List<Match3Node>();
		
		int bufferMatches = 0;

		for (int y = 0; y < gridHeight; y++)
		{
			for (int x = 0; x < gridWidth; x++)
			{

				if (x + 2 < gridWidth && j < 0 && grid[x + 1, y].id == grid[x, y].id && grid[x + 2, y].id == grid[x, y].id)
				{

					j = grid[x, y].id;
				}

				if (j == grid[x, y].id)
				{
					matches.Add(grid[x, y]);
					bufferMatches++;
				}
				else
				{
					if (bufferMatches > 0) score.addScore(bufferMatches);
					bufferMatches = 0;
					j = -1;
				}
				
			}


			j = -1;
		}

		j = -1;

		for (int y = 0; y < gridWidth; y++)
		{
			for (int x = 0; x < gridHeight; x++)
			{
				if (x + 2 < gridHeight && j < 0 && grid[y, x + 1].id == grid[y, x].id && grid[y, x + 2].id == grid[y, x].id)
				{
					j = grid[y, x].id;
				}

				if (j == grid[y, x].id)
				{
					matches.Add(grid[y, x]);
					bufferMatches++;
                }
                else
				{
					if(bufferMatches > 0) score.addScore(bufferMatches);
                    bufferMatches = 0;
                    j = -1;
				}
			}

			j = -1;
		}

		return (matches.Count > 0) ? true : false;
	}

	/// <summary>
	/// Создание поля
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="prefab">Тип фшки</param>
	/// <param name="width">Ширина поля</param>
	/// <param name="height">Высота поля</param>
	/// <param name="spacing">Расстояние между фишками</param>
	/// <returns></returns>
	private T[,] Create2DGrid<T>(T prefab, int width, int height, float spacing) where T : MonoBehaviour
	{
		T[,] field = new T[width, height];

		float posX = (width + 1) * -spacing / 2f;
		float posY = (height - 1) * spacing / 2f;

		float Xreset = posX;

		int z = 0;

		for (int y = 0; y < height; y++)
		{
			for (int x = 0; x < width; x++)
			{
				posX += spacing;
				field[x, y] = Instantiate(prefab, new Vector2( 
					transform.position.x + posX * prefab.transform.localScale.x, 
					transform.position.y + posY * prefab.transform.localScale.y), 
					Quaternion.identity) as T;
				field[x, y].name = "Node-" + z;
				z++;
			}
			posY -= spacing;
			posX = Xreset;
		}

		return field;
	}
}