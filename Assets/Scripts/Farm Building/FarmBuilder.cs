using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FarmBuilder : MonoBehaviour
{
    [Header("Status")]
    public bool farmBuilderActive;

    [Header("Placeables")]
    public Vector3 targetLocation;
    public GameObject objectBeingPlaced;

    private void Update()
    {
        if (farmBuilderActive)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit) && !EventSystem.current.IsPointerOverGameObject())
            {
                if (hit.collider != null)
                {
                    targetLocation = hit.point;
                    targetLocation.x = Mathf.Round(targetLocation.x) + 0.5f;
                    targetLocation.z = Mathf.Round(targetLocation.z) + 0.5f;

                    objectBeingPlaced.transform.position = targetLocation;

                }
                else
                {

                }
            }
        }
    }
}
