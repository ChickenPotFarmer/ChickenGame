using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableArea : MonoBehaviour
{
    public MeshRenderer meshRenderer;
    //public BoxCollider boxCollider;
    public bool active;

    public static PlaceableArea instance;
    [HideInInspector]
    public GameObject placeableArea;

    private void Awake()
    {
        instance = this;
        placeableArea = gameObject;
    }

    public void ToggleArea(bool _active)
    {
        if (_active)
        {
            active = true;
            meshRenderer.enabled = true;
            //boxCollider.enabled = true;
        }
        else
        {
            active = false;
            meshRenderer.enabled = false;
            //boxCollider.enabled = false;
        }    
    }
}
