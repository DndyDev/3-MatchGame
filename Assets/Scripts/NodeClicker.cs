using System.Collections;
using System.Collections.Generic;
using UnityEngine;
    class NodeClicker: MonoBehaviour 
    {
		[SerializeField] Match3Control playground;
		[SerializeField] private LayerMask layerMask;
		[SerializeField] private Timer timer;

	private void Update()
    {
		
        if (playground.IsLines || playground.IsMove || timer.TimeRemeining < 0) return;

        if (playground.Last == null)
		{
			MoveNode();
		}
		else
		{
			playground.MoveCurrent();
		}
	}

    /// <summary>
    /// Управление игрока
    /// </summary>
    void MoveNode()
	{
		if (Input.GetMouseButtonDown(0) && !playground.IsMode)
		{
			RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, layerMask);

			if (hit.transform != null && playground.Current == null)
			{
				playground.Current = hit.transform.GetComponent<Match3Node>();
				playground.SetNode(playground.Current, true);
			}
			else if (hit.transform != null && playground.Current != null)
			{
				playground.Last = hit.transform.GetComponent<Match3Node>();

				if (playground.Last != null && !playground.Last.ready)
				{
					playground.SetNode(playground.Current, false);
					playground.SetNode(playground.Last, true);
					playground.Current = playground.Last;
					playground.Last = null;
					return;
				}

				playground.CurrentPos = playground.Current.transform.position;
				playground.LastPos = playground.Last.transform.position;
				playground.IsMode = true;
			}
		}
	}

}