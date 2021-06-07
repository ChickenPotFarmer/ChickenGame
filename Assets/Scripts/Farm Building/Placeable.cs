using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placeable : MonoBehaviour
{
    [Header("Materials")]
    public MeshRenderer meshRenderer;
    public Material okMat;
    public Material nopeMat;

    private void OnTriggerEnter(Collider other)
    {
        meshRenderer.material = nopeMat;

    }

    private void OnTriggerExit(Collider other)
    {
        meshRenderer.material = okMat;

    }

    public void PlaceObject()
    {
        meshRenderer.enabled = false;
    }
}
