using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    class PieceNode :MonoBehaviour
    {
    [SerializeField] private SpriteRenderer sprite;
    private int id; public int Id => id;
    private bool ready; public bool Ready => ready;
    private int x; public int X => x;
    private int y; public int Y => y;

    private bool move; public bool Move => move;
}
