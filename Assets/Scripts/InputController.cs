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

    [Header("Cache")]
    [SerializeField]
    private ThirdPersonController thirdPersonController;


    private WateringCan wateringCan;
    private Trimmer trimmer;
    private SeedCannon seedCannon;
    private BuyerController buyerController;
    private LaptopController laptopController;
    private ToDoController toDoController;
    private ChickenController chickenController;
    private InventoryController inventoryController;
    private RadialMenu radialMenu;
    private MapController mapController;
    private WeedPlant selectedPlant;
    private TutorialPlant selectedTutorialPlant;
    private WeedPlant foundPlant;
    private TutorialPlant foundTutorialPlant;
    private SplatGun splatGun;
    private string debugTag;

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
        if (!splatGun)
            splatGun = SplatGun.instance.splatGun.GetComponent<SplatGun>();
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
                thirdPersonController.OverrideUnlock(); // replace
            }
            else if (Input.GetKeyUp("r"))
            {
                radialMenuOn = false;
                radialMenu.SetMenuActive(false);

                if (!splatGun.cannonOn) // replace
                    thirdPersonController.OverrideLock();
                else
                    thirdPersonController.SplatCannonLock();

            }
            else if (Input.GetKeyDown("m"))
            {
                mapController.ToggleMap();
            }

            // TOOLS
            if (splatGun.cannonOn)
            {
                if (InteractWith())
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                    if (Physics.Raycast(ray, out RaycastHit hit) && !EventSystem.current.IsPointerOverGameObject())
                    {
                        splatGun.Fire(hit.point);
                    }
                }
            }
            else if (trimmer.trimmerOn)
            {
                if (InteractWith())
                {
                    trimmer.TrimAndHarvestPlant();
                }
            }

            if (!radialMenuOn)
            {
                // Mouse Control
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out RaycastHit hit) && !EventSystem.current.IsPointerOverGameObject())
                {
                    if (hit.collider != null)
                    {
                        string tagId;

                        if (hit.collider.CompareTag("Placeable"))
                            tagId = hit.collider.transform.parent.tag;
                        else
                            tagId = hit.collider.gameObject.tag;

                        debugTag = tagId;

                        PlanterUnhover();
                        BuyerUnhover();

                        //print(tagId);

                        switch (tagId)
                        {
                            case "Planter Hub":
                            case "Trimmer Hub":
                                if (InteractWith())
                                {
                                    try
                                    {
                                        hit.collider.gameObject.GetComponent<ObjectInventory>().SetPanelActive(true);
                                    }
                                    catch
                                    {
                                        Debug.LogWarning("Object inventory not found");
                                    }
                                }
                                break;

                            case "Buyer":
                                buyerController.hoveringOver = hit.collider.gameObject.GetComponentInParent<Buyer>();
                                buyerController.hoveringOver.SetHoverInfoActive(true);

                                if (InteractWith())
                                {
                                    buyerController.hoveringOver.OpenBuyerPanel();
                                }

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

                                //BuyerUnhover();
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

                                //BuyerUnhover();
                                break;

                            case "Weed Plant":
                                foundPlant = hit.collider.gameObject.GetComponent<WeedPlant>();

                                // Handle unplanted weed plant
                                if (!foundPlant.isPlanted && seedCannon.cannonOn)
                                {
                                    if (selectedPlant != null)
                                    {
                                        if (foundPlant != selectedPlant && !selectedPlant.isPlanted)
                                        {
                                            selectedPlant.SetNone();
                                        }
                                    }
                                    selectedPlant = foundPlant;
                                    selectedPlant.SetFullGrown();
                                }

                                if (InteractWith())
                                {
                                    if (foundPlant.fullyGrown)
                                    {
                                        //if (trimmer.TrimmerIsOn())
                                        //{
                                        //    if (!foundPlant.trimmed)
                                        //    {
                                        //        //trimmer.TargetForTrim(foundPlant);
                                        //        //trimmer.TrimPlant();
                                        //        //Debug.Log("targetted plant for trim", foundPlant);
                                        //        //chickenController.SetNewDestination(hit.point);
                                        //    }
                                        //    else if (!foundPlant.harvested)
                                        //    {
                                        //        foundPlant.TargetForHarvest();
                                        //        Debug.Log("targetted plant for harvest", foundPlant);

                                        //        chickenController.SetNewDestination(hit.point);
                                        //    }
                                        //    else if (foundPlant.harvested)
                                        //    {
                                        //        foundPlant.SetHarvestPanelActive(true);
                                        //    }

                                        //}
                                        //else
                                        //{
                                        //    if (foundPlant.harvested)
                                        //        foundPlant.SetHarvestPanelActive(true);
                                        //}
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

                                //BuyerUnhover();
                                break;

                            case "Tutorial Plant":
                                foundTutorialPlant = hit.collider.gameObject.GetComponent<TutorialPlant>();

                                // Handle unplanted weed plant
                                if (!foundTutorialPlant.isPlanted && seedCannon.cannonOn)
                                {
                                    if (selectedTutorialPlant != null)
                                    {
                                        if (foundTutorialPlant != selectedTutorialPlant && !selectedTutorialPlant.isPlanted)
                                        {
                                            selectedTutorialPlant.SetNone();
                                        }
                                    }
                                    selectedTutorialPlant = foundTutorialPlant;
                                    selectedTutorialPlant.SetFullGrown();
                                }

                                if (InteractWith())
                                {
                                    if (foundTutorialPlant.fullyGrown)
                                    {
                                        if (trimmer.TrimmerIsOn())
                                        {
                                            if (!foundTutorialPlant.trimmed)
                                            {
                                                trimmer.TargetForTrim(foundTutorialPlant);
                                                chickenController.SetNewDestination(hit.point);
                                            }
                                            else if (!foundTutorialPlant.harvested)
                                            {
                                                foundTutorialPlant.TargetForHarvest();
                                                chickenController.SetNewDestination(hit.point);
                                            }
                                            else if (foundTutorialPlant.harvested)
                                            {
                                                foundTutorialPlant.SetHarvestPanelActive(true);
                                            }

                                        }
                                        else
                                        {
                                            if (foundTutorialPlant.harvested)
                                                foundTutorialPlant.SetHarvestPanelActive(true);
                                        }
                                    }
                                    else
                                    {
                                        if (seedCannon.cannonOn && !foundTutorialPlant.hasSeed)
                                        {
                                            seedCannon.FireCannon(foundTutorialPlant.transform);
                                        }

                                        else if (wateringCan.waterCanOn && foundTutorialPlant.isPlanted)
                                        {
                                            wateringCan.TargetForWater(foundTutorialPlant);
                                            chickenController.SetNewDestination(hit.point);

                                        }

                                        else if (trimmer.trimmerOn && foundTutorialPlant.isPlanted)
                                        {
                                            foundTutorialPlant.DestroyPlant();
                                        }
                                    }
                                }

                                //BuyerUnhover();
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
                        BuyerUnhover();
                        PlanterUnhover();
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

    private void PlanterUnhover()
    {
        if (selectedTutorialPlant != null)
        {
            selectedTutorialPlant.SetNone();
            selectedTutorialPlant = null;
        }

        if (selectedPlant != null)
        {
            selectedPlant.SetNone();
            selectedPlant = null;
        }
    }

    public void BuyerUnhover()
    {
        if (buyerController.hoveringOver != null)
        {
            buyerController.hoveringOver.SetHoverInfoActive(false);
            buyerController.hoveringOver = null;
        }
    }

    public void TurnOffAllTools()
    {
        trimmer.ToggleTrimmer(false);
        seedCannon.ToggleCannon(false);
        wateringCan.ToggleWaterCan(false);
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(65, 10, 100, 50), debugTag);
    }
}
