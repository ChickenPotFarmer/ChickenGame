using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DestroyHighlight : MonoBehaviour
{
    private MeshRenderer meshR;

    public WeedPlant plant;
    public bool highlightActive;

    public static List<DestroyHighlight> masterHighlightList;
    private void Start()
    {
        if (!meshR)
            meshR = GetComponent<MeshRenderer>();

        ActivateHighlight(false);

    }

    private void Update()
    {
        if (highlightActive)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.CompareTag("Weed Plant"))
                {
                    if (hit.collider.GetComponent<WeedPlant>() != plant)
                    {
                        ActivateHighlight(false);
                    }
                }
                else
                    ActivateHighlight(false);
                

            }
        }
    }

    public void ActivateHighlight(bool _active)
    {
        if (_active)
        {
            highlightActive = true;
            meshR.enabled = true;

        }
        else
        {
            highlightActive = false;
            meshR.enabled = false;

        }
    }
}
