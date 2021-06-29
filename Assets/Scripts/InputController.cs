using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InputController : MonoBehaviour
{
    [Header("Status")]
    public bool farmBuilderActive;
    public bool radialMenuOn;
    public bool fugitive;
    public bool mapActive;


    private WateringCan wateringCan;
    private Trimmer trimmer;
    private SeedCannon seedCannon;
    private BuyerController buyerController;
    private DryerController dryerController;
    private LaptopController laptopController;
    private Planter planterController;
    private ToDoController toDoController;
    private ChickenController chickenController;
    private InventoryController inventoryController;
    private RadialMenu radialMenu;
    private MapController mapController;


    public static InputController instance;
    [HideInInspector]
    public GameObject inputController;

    private void Awake()
    {
        instance = this;
        inputController = gameObject;
    }

    private void Start()
    {
        if (!buyerController)
            buyerController = BuyerController.instance.buyerContoller.GetComponent<BuyerController>();
        if (!laptopController)
            laptopController = LaptopController.instance.laptopController.GetComponent<LaptopController>();
        if (!planterController)
            planterController = Planter.instance.planter.GetComponent<Planter>();
        if (!toDoController)
            toDoController = ToDoController.instance.toDoController.GetComponent<ToDoController>();
        if (!chickenController)
            chickenController = ChickenController.instance.chickenController.GetComponent<ChickenController>();
        if (!seedCannon)
            seedCannon = SeedCannon.instance.seedCannon.GetComponent<SeedCannon>();
        if (!trimmer)
            trimmer = Trimmer.instance.trimmer.GetComponent<Trimmer>();
        if (!wateringCan)
            wateringCan = WateringCan.instance.waterCan.GetComponent<WateringCan>();
        if (!radialMenu)
            radialMenu = GetComponent<RadialMenu>();
        if (!mapController)
            mapController = MapController.instance.mapController.GetComponent<MapController>();
        if (!inventoryController)
            inventoryController = InventoryController.instance.inventoryController.GetComponent<InventoryController>();
    }

    private void Update()
    {
        if (!farmBuilderActive)
        {
            // Keyboard Controls
            if (Input.GetKeyDown(KeyCode.Tab))
                inventoryController.ToggleInventoryPanel();

            else if (Input.GetKeyDown(KeyCode.T))
                toDoController.ToggleToDoPanel();

            else if (Input.GetKeyDown("1"))
            {
                if (seedCannon.cannonOn)
                {
                    seedCannon.ToggleCannon();
                }
                else
                {
                    TurnOffAllTools();
                    seedCannon.ToggleCannon();
                }
            }

            else if (Input.GetKeyDown("2"))
            {
                if (trimmer.trimmerOn)
                {
                    trimmer.ToggleTrimmer();
                }
                else
                {
                    TurnOffAllTools();
                    trimmer.ToggleTrimmer();
                }
            }

            else if (Input.GetKeyDown("3"))
            {
                if (wateringCan.waterCanOn)
                {
                    wateringCan.ToggleWaterCan();
                }
                else
                {
                    TurnOffAllTools();
                    wateringCan.ToggleWaterCan();
                }
            }
            else if (Input.GetKeyDown("r"))
            {
                radialMenuOn = true;
                radialMenu.SetMenuActive(true);
            }
            else if (Input.GetKeyUp("r"))
            {
                radialMenuOn = false;
                radialMenu.SetMenuActive(false);
            }
            else if (Input.GetKeyDown("m"))
            {
                mapController.ToggleMap();
            }

            if (!radialMenuOn)
            {
                // Mouse Control
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit) && !EventSystem.current.IsPointerOverGameObject())
                {
                    if (hit.collider != null)
                    {
                        string tagId;

                        if (hit.collider.CompareTag("Placeable"))
                            tagId = hit.collider.transform.parent.tag;
                        else
                            tagId = hit.collider.gameObject.tag;

                        print(tagId);

                        switch (tagId)
                        {
                            case "Planter Hub":
                                if (InteractWith())
                                {
                                    hit.collider.gameObject.GetComponentInParent<PlanterChickHub>().SetPanelActive(true);
                                }
                                break;

                            case "Buyer":
                                buyerController.hoveringOver = hit.collider.gameObject.GetComponentInParent<Buyer>();
                                buyerController.hoveringOver.SetHoverInfoActive(true);

                                if (InteractWith())
                                {
                                    buyerController.hoveringOver.OpenBuyerPanel();
                                }

                                PlanterUnhover();
                                break;

                            case "Dryer":
                                DryerController dryer = hit.collider.gameObject.GetComponentInParent<DryerController>();
                                if (InteractWith() && dryer.clickActive)
                                {
                                    if (dryer.placed)
                                    {
                                        dryer.SetDryerPanelActive(true);
                                        dryer.clickActive = false; // what is this???
                                    }
                                }

                                PlanterUnhover();
                                BuyerUnhover();
                                break;

                            case "Laptop":
                                if (laptopController.chickenInRange)
                                {
                                    if (InteractWith() && laptopController.clickActive)
                                    {
                                        laptopController.SetLaptopPanelActive(true);
                                        laptopController.clickActive = false;
                                    }
                                }
                                PlanterUnhover();
                                BuyerUnhover();
                                break;

                            case "Weed Plant":
                                WeedPlant foundPlant = hit.collider.gameObject.GetComponent<WeedPlant>();

                                // Handle unplanted weed plant
                                if (!foundPlant.isPlanted && seedCannon.cannonOn)
                                {
                                    if (planterController.selectedPlant != null)
                                    {
                                        if (foundPlant != planterController.selectedPlant && !planterController.selectedPlant.isPlanted)
                                        {
                                            planterController.selectedPlant.SetNone();
                                        }
                                    }
                                    planterController.selectedPlant = foundPlant;
                                    planterController.selectedPlant.SetFullGrown();
                                }

                                if (InteractWith())
                                {
                                    if (foundPlant.fullyGrown)
                                    {
                                        if (trimmer.TrimmerIsOn())
                                        {
                                            if (!foundPlant.trimmed)
                                            {
                                                trimmer.TargetForTrim(foundPlant);
                                                chickenController.SetNewDestination(hit.point);
                                            }
                                            else if (!foundPlant.harvested)
                                            {
                                                foundPlant.TargetForHarvest();
                                                chickenController.SetNewDestination(hit.point);
                                            }
                                            else if (foundPlant.harvested)
                                            {
                                                foundPlant.SetHarvestPanelActive(true);
                                            }

                                        }
                                        else
                                        {
                                            if (foundPlant.harvested)
                                                foundPlant.SetHarvestPanelActive(true);
                                        }
                                    }
                                    else
                                    {
                                        if (seedCannon.cannonOn && !foundPlant.hasSeed)
                                        {
                                            seedCannon.FireCannon(foundPlant.transform);
                                        }

                                        else if (wateringCan.waterCanOn && foundPlant.isPlanted)
                                        {
                                            wateringCan.TargetForWater(foundPlant);
                                            chickenController.SetNewDestination(hit.point);

                                        }

                                        else if (trimmer.trimmerOn && foundPlant.isPlanted)
                                        {

                                            foundPlant.DestroyPlant();

                                            
                                        }
                                    }
                                }

                                BuyerUnhover();
                                break;

                            case "Storage Container":
                                StorageCrate crate = hit.collider.GetComponentInParent<StorageCrate>();

                                if (crate && InteractWith())
                                {
                                    if (crate.placed)
                                    {
                                        crate.OpenCrate(true);
                                        print("opened");
                                    }
                                }

                                break;


                            default:

                                PlanterUnhover();
                                BuyerUnhover();
                                break;

                        }

                    }
                    else
                    {
                        PlanterUnhover();
                        BuyerUnhover();
                    }
                }

                if (Input.GetButtonDown("Navigate"))
                {
                    chickenController.SetNewDestination(hit.point);
                }
            }
        }
    }

    public bool InteractWith()
    {
        bool clicked = false;

        if (Input.GetButtonDown("Interact") && !fugitive && !mapActive)
        {
            clicked = true;
        }

        return clicked;
    }

    public void BuyerUnhover()
    {
        if (buyerController.hoveringOver != null)
        {
            buyerController.hoveringOver.SetHoverInfoActive(false);
            buyerController.hoveringOver = null;
        }
    }

    public void PlanterUnhover()
    {
        if (planterController.selectedPlant != null && !planterController.selectedPlant.isPlanted)
        {
            planterController.selectedPlant.SetNone();
            planterController.selectedPlant = null;
        }
    }

    public void TurnOffAllTools()
    {
        trimmer.ToggleTrimmer(false);
        seedCannon.ToggleCannon(false);
        wateringCan.ToggleWaterCan(false);
    }
}
