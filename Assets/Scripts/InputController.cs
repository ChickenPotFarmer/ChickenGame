using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    private BuyerController buyerController;
    private DryerController dryerController;
    private LaptopController laptopController;
    private Planter planterController;
    private ToDoController toDoController;
    private ChickenController chickenController;

    private void Start()
    {
        if (!buyerController)
            buyerController = BuyerController.instance.buyerContoller.GetComponent<BuyerController>();
        if (!dryerController)
            dryerController = DryerController.instance.dryerController.GetComponent<DryerController>();
        if (!laptopController)
            laptopController = LaptopController.instance.laptopController.GetComponent<LaptopController>();
        if (!planterController)
            planterController = Planter.instance.planter.GetComponent<Planter>();
        if (!toDoController)
            toDoController = ToDoController.instance.toDoController.GetComponent<ToDoController>();
        if (!chickenController)
            chickenController = ChickenController.instance.chickenController.GetComponent<ChickenController>();
    }

    private void Update()
    {
        // Keyboard Controls
        if (Input.GetKeyDown(KeyCode.Tab))
            toDoController.ToggleToDoPanel();

        // Mouse Control
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider != null)
            {
                string tagId = hit.collider.gameObject.tag;

                switch (tagId)
                {
                    case "Buyer":
                        buyerController.hoveringOver = hit.collider.gameObject.GetComponent<Buyer>();
                        buyerController.hoveringOver.SetHoverInfoActive(true);

                        if (Input.GetButtonDown("Interact"))
                        {
                            buyerController.hoveringOver.DeliverWeed();
                        }

                        PlanterUnhover();
                        break;

                    case "Dryer Pile":
                        if (Input.GetButtonDown("Interact") && dryerController.clickActive)
                        {

                            if (!dryerController.isDry)
                            {
                                dryerController.SetDryerPanelActive(true);
                                dryerController.clickActive = false;
                            }
                            else
                            {
                                dryerController.Harvest();
                            }
                        }

                        PlanterUnhover();
                        BuyerUnhover();
                        break;

                    case "Laptop":
                        if (laptopController.chickenInRange)
                        {
                            if (Input.GetButtonDown("Interact") && laptopController.clickActive)
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
                        if (!foundPlant.isPlanted)
                        {
                            if (planterController.selectedPlant != null)
                            {
                                if (foundPlant != planterController.selectedPlant)
                                {
                                    planterController.selectedPlant.SetNone();
                                }
                            }
                            planterController.selectedPlant = foundPlant;
                            planterController.selectedPlant.SetFullGrown();
                        }

                        if (Input.GetButtonDown("Interact"))
                        {
                            if (foundPlant.fullyGrown)
                            {
                                foundPlant.Harvest();
                            }
                        }

                        if (planterController.planterOn && planterController.seeds != 0)
                        {
                            if (planterController.selectedPlant != null)
                            {
                                if (Input.GetButtonDown("Interact") && !planterController.selectedPlant.selected)
                                {
                                    planterController.selectedPlant.selected = true;
                                    planterController.SetNewPlantPanelActive(true);
                                    planterController.planterOn = false;
                                }
                            }
                        }

                        BuyerUnhover();
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


            if (Input.GetButtonDown("Navigate"))
            {
                chickenController.SetNewDestination(hit.point);
            }
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

    public void PlanterUnhover()
    {
        if (planterController.selectedPlant != null)
        {
            planterController.selectedPlant.SetNone();
            planterController.selectedPlant = null;
        }
    }
}
