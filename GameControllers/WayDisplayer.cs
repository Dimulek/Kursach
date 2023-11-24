using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayDisplayerer : MonoBehaviour
{
    [SerializeField]
    private LineRenderer lineRenderer;

    public void updateWay(in List<Vector2> wayToDeath)
    {
        lineRenderer.positionCount = wayToDeath.Count;
        for (int i =0; i < wayToDeath.Count; ++i)
            lineRenderer.SetPosition(i, wayToDeath[i]);
    }
}
