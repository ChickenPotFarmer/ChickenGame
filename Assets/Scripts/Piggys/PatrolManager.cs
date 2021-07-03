using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolManager : MonoBehaviour
{
    [Header("Debug Line")]
    public LineRenderer lineRenderer;

    public static PatrolManager instance;
    [HideInInspector]
    public GameObject patrolManager;

    private void Awake()
    {
        instance = this;
        patrolManager = gameObject;
    }
}
