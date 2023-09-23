using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    class PieceNode :MonoBehaviour
    {
    [SerializeField] private SpriteRenderer renderer; public SpriteRenderer Renderer { get { return renderer;} set { renderer = value; } }
    private int id; public int Id { get { return id; } set { id = value; } }
    private bool ready { get; set; } public bool Ready { get { return ready; } set { ready = value; } }
    private bool visited { get; set; } public bool Visited { get { return visited; } set { visited = value; } }
    private int x { get; set; } public int X { get { return x; } set { x = value; } }
    private int y { get; set; } public int Y { get { return y; } set { y = value; } }

    private bool move { get; set; } public bool Move {get { return move; } set { move = value; } }
}
