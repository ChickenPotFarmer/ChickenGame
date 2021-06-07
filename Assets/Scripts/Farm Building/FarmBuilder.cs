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
    private bool objectFlipped;

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
                    targetLocation.x = Mathf.Round(targetLocation.x);
                    targetLocation.z = Mathf.Round(targetLocation.z);


                    if (Input.GetKeyDown("r"))
                    {
                        var lookPos = targetLocation;
                        lookPos.y = 0;
                        var rotation = Quaternion.LookRotation(lookPos);

                        if (!objectFlipped)
                        {
                            objectFlipped = true;
                            rotation *= Quaternion.Euler(0, 90, 0); // this adds a 90 degrees Y rotation
                        }
                        else
                        {
                            objectFlipped = false;
                            rotation *= Quaternion.Euler(0, -90, 0); // this adds a 90 degrees Y rotation
                        }


                        objectBeingPlaced.transform.rotation = Quaternion.RotateTowards(objectBeingPlaced.transform.rotation, rotation, 90);
                        //objectBeingPlaced.transform.rotation = Quaternion.RotateTowards(objectBeingPlaced;

                    }
                    else
                        objectBeingPlaced.transform.position = targetLocation;

                }
                else
                {

                }
            }
        }
    }
}
