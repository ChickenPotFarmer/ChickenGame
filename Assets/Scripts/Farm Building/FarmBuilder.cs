using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FarmBuilder : MonoBehaviour
{
    [Header("Status")]
    public bool farmBuilderActive;

    [Header("Placeables")]
    public GameObject[] placeables;

    [Header("Placeable Setup")]
    public Vector3 targetLocation;
    public GameObject objectBeingPlaced;
    private bool objectFlipped;

    private InputController inputController;
    private PlaceableArea placeableArea;

    public static FarmBuilder instance;
    [HideInInspector]
    public GameObject farmBuilder;

    private void Awake()
    {
        instance = this;
        farmBuilder = gameObject;
    }

    private void Start()
    {
        if (!inputController)
            inputController = InputController.instance.inputController.GetComponent<InputController>();

        if (!placeableArea)
            placeableArea = PlaceableArea.instance.placeableArea.GetComponent<PlaceableArea>();
    }

    private void Update()
    {

        if (farmBuilderActive && objectBeingPlaced != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit) && !EventSystem.current.IsPointerOverGameObject())
            {
                if (hit.collider != null)
                {
                    if (hit.collider.CompareTag("Placeable Area"))
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
                                ToggleFarmBuilder(false);
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
                        print("nope");
                    }
                }
            }
        }
    }

    public void SelectPlaceable(GameObject _placeable)
    {
        ToggleFarmBuilder(true);
        GameObject newPlaceable = Instantiate(_placeable);
        objectBeingPlaced = newPlaceable;
    }

    public void ToggleFarmBuilder(bool _active)
    {
        if (_active)
        {
            farmBuilderActive = true;
            placeableArea.ToggleArea(true);
        }
        else
        {
            farmBuilderActive = false;
            placeableArea.ToggleArea(false);
        }
    }
}
