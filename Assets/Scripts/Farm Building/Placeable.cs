using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placeable : MonoBehaviour
{
    [Header("Things to activate")]
    public Transform activateObjectsParent;
    public List<GameObject> activeObjects;

    [Header("Status")]
    public bool placementOk = true;

    [Header("Materials")]
    public MeshRenderer meshRenderer;
    public Material okMat;
    public Material nopeMat;

    private void OnTriggerEnter(Collider other)
    {
        meshRenderer.material = nopeMat;
        placementOk = false;

    }

    private void OnTriggerExit(Collider other)
    {
        meshRenderer.material = okMat;
        placementOk = true;
    }

    public void PlaceObject()
    {
        meshRenderer.enabled = false;

        if (activateObjectsParent != null)
        {
            for (int i = 0; i < activateObjectsParent.childCount; i++)
            {
                activeObjects.Add(activateObjectsParent.GetChild(i).gameObject);
            }
        }

        for (int i = 0; i < activeObjects.Count; i++)
        {
            activeObjects[i].SetActive(true);
        }
    }
}
