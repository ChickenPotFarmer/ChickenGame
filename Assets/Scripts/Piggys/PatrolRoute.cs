using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolRoute : MonoBehaviour
{
    [Header("Debug Line")]
    public LineRenderer lineRenderer;
    public bool updateLine;

    [Header("Setup")]
    public Transform waypointsParent;

    private void Start()
    {
        lineRenderer.enabled = false;
    }

}
