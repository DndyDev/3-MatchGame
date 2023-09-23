using System.Collections;
using UnityEngine;

public class Match3Node : MonoBehaviour
{

	public SpriteRenderer sprite; // спрайт узла
	public GameObject highlight; // объект подсветки узла
	public int id { get; set; }
	public bool ready { get; set; }
	public int x { get; set; }
	public int y { get; set; }

    public bool move { get; set; }
}