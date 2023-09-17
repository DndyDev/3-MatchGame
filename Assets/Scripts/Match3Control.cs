using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pool;

public class Match3Control : MonoBehaviour
{

	[SerializeField] private float speed = 400; // �������� �������� ��������
	[SerializeField] private float destroyTimeout = .5f; // ����� � ��������, ����� ��� ��� ���������� ����������
	[SerializeField] private LayerMask layerMask; // ����� ���� (�������)
	[SerializeField] private Sprite[] sprites; // ����� ������/id
	[SerializeField] private int gridWidth = 7; // ������ �������� ����
	[SerializeField] private int gridHeight = 10; // ������ �������� ����
	[SerializeField] private Match3Node sampleObject; // ������� ���� (������)
	[SerializeField] private float spacing = 1.3f; // ������ ���� (������ � ������)

	private Match3Pool<Match3Node> nodePool;
	private Match3Node[,] grid;
	private Match3Node[] nodeArray;
	private Vector3[,] position;
	private Match3Node current, last;
	private Vector3 currentPos, lastPos;
	private List<Match3Node> matches;
	private bool isLines, isMove, isMode;
	private float timeout;

	void Start()
	{
		nodePool = new Match3Pool<Match3Node>();
		grid = Create2DGrid<Match3Node>(sampleObject,gridWidth, gridHeight, spacing);
        InittiatePlayground();
    }

	void Update()
	{
        DestroyMatches();

        MoveNodes();

        if (isLines || isMove) return;

        if (last == null)
        {
            Control();
        }
        else
        {
            MoveCurrent();
        }
    }
	/// <summary>
	/// ���������� �������� ����
	/// </summary>
    void InittiatePlayground()
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
	/// �������� ����������
	/// </summary>
	void DestroyMatches()
	{
		if (!isLines) return;

		timeout += Time.deltaTime;

		if (timeout > destroyTimeout)
		{
			for (int i = 0; i < matches.Count; i++)
			{
				// ����� ����� ������������ ���� +1
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
	/// ����� �����
	/// </summary>
	void MoveNodes() // 
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
	/// ���������� ����� �����
	/// </summary>
	/// <param name="pos">������� �����</param>
	/// <returns></returns>
	Match3Node GetNewNode(Vector3 pos)
	{
		int j = Random.Range(0, sprites.Length);		
		return nodePool.GetObject(j, sprites[j], pos);
	}

	/// <summary>
	/// ���������� ���� 
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
	/// ����������� ���� ������
	/// </summary>
	void MoveCurrent()
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
	/// �������� ���� �� ������ � ������� �����
	/// </summary>
	/// <param name="node">�����</param>
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
	/// ������ �� ����������� �������� �����
	/// </summary>
	/// <param name="node">�����</param>
	/// <param name="value">����</param>
	void SetNode(Match3Node node, bool value) 
	{
		if (node == null) return;

		if (node.x - 1 >= 0) grid[node.x - 1, node.y].ready = value;
		if (node.y - 1 >= 0) grid[node.x, node.y - 1].ready = value;
		if (node.x + 1 < gridWidth) grid[node.x + 1, node.y].ready = value;
		if (node.y + 1 < gridHeight) grid[node.x, node.y + 1].ready = value;
	}

	/// <summary>
	/// ���������� ������
	/// </summary>
	void Control()
	{
		if (Input.GetMouseButtonDown(0) && !isMode)
		{
			RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, layerMask);

			if (hit.transform != null && current == null)
			{
				current = hit.transform.GetComponent<Match3Node>();
				SetNode(current, true);
				current.highlight.SetActive(true);
			}
			else if (hit.transform != null && current != null)
			{
				last = hit.transform.GetComponent<Match3Node>();

				if (last != null && !last.ready)
				{
					current.highlight.SetActive(false);
					last.highlight.SetActive(true);
					SetNode(current, false);
					SetNode(last, true);
					current = last;
					last = null;
					return;
				}

				current.highlight.SetActive(false);
				currentPos = current.transform.position;
				lastPos = last.transform.position;
				isMode = true;
			}
		}
	}

	/// <summary>
	/// �������� ������������ �� ����������� � ���������
	/// </summary>
	/// <returns></returns>
	bool IsMatche()
	{
		int j = -1;

		matches = new List<Match3Node>();

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
				}
				else
				{
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
				}
				else
				{
					j = -1;
				}
			}

			j = -1;
		}

		return (matches.Count > 0) ? true : false;
	}

	/// <summary>
	/// �������� ����
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="prefab">��� ����</param>
	/// <param name="width">������ ����</param>
	/// <param name="height">������ ����</param>
	/// <param name="spacing">���������� ����� �������</param>
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