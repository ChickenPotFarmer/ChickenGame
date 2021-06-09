using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FarmBuilder : MonoBehaviour
{
    [Header("Status")]
    public bool farmBuilderActive;
    public bool placementActive;

    [Header("Placeables")]
    public GameObject[] placeables;

    [Header("Placeable Setup")]
    public Vector3 targetLocation;
    public GameObject objectBeingPlaced;
    private bool objectFlipped;

    private InputController inputController;

    private void Start()
    {
        if (!inputController)
            inputController = InputController.instance.inputController.GetComponent<InputController>();
    }

    private void Update()
    {
        if (Input.GetKeyDown("["))
            SelectPlaceable(0);
        else if (Input.GetKeyDown("]"))
            SelectPlaceable(1);

        if (placementActive && objectBeingPlaced != null)
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
                        if (!objectFlipped)
                        {
                            objectFlipped = true;
                            objectBeingPlaced.transform.Rotate(new Vector3(0, 90, 0));
                        }
                        else
                        {
                            objectFlipped = false;
                            objectBeingPlaced.transform.Rotate(new Vector3(0, -90, 0));

                        }

                    }
                    else if (Input.GetButtonDown("Interact"))
                    {
                        Placeable placeableComp = objectBeingPlaced.GetComponentInChildren<Placeable>();

                        if (placeableComp.placementOk)
                        {
                            objectBeingPlaced.transform.position = targetLocation;
                            placeableComp.PlaceObject();
                            objectBeingPlaced = null;
                            placementActive = false;
                            inputController.farmBuilderActive = false;
                        }
                        else
                        {
                            //play sound
                        }

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

    public void SelectPlaceable(int _placeableInt)
    {
        placementActive = true;
        GameObject newPlaceable = Instantiate(placeables[_placeableInt]);
        objectBeingPlaced = newPlaceable;
    }
}
