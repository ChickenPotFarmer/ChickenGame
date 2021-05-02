using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DryerController : MonoBehaviour
{
    public float inventory;
    public float maxInventory;
    public bool isDry;
    public float dryTime;
    public Slider dryBar;
    public CanvasGroup dryBarCg;
    public CanvasGroup dryerPanel;
    public bool clickActive;
    public bool chickenInRange;

    private ChickenController chicken;
    private InventoryController inventoryController;

    public static DryerController instance;
    [HideInInspector]
    public GameObject dryerController;

    private void Awake()
    {
        instance = this;
        dryerController = gameObject;
    }

    private void Start()
    {
        if (!inventoryController)
            inventoryController = InventoryController.instance.inventoryController.GetComponent<InventoryController>();

        if (!chicken)
            chicken = ChickenController.instance.chickenController.GetComponent<ChickenController>();

        StartCoroutine(InRangeCheck());
    }

    //private void Update()
    //{
    //    if (chickenInRange)
    //    {
    //        if (Input.GetMouseButtonDown(0) && clickActive)
    //        {
    //            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //            RaycastHit hit;

    //            if (Physics.Raycast(ray, out hit))
    //            {
    //                if (hit.collider != null)
    //                {
    //                    if (hit.collider.gameObject == gameObject)
    //                    {
    //                        if (!isDry)
    //                        {
    //                            SetDryerPanelActive(true);
    //                            clickActive = false;
    //                        }
    //                        else
    //                        {
    //                            Harvest();
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //    }
    //}

    IEnumerator InRangeCheck()
    {
        do
        {
            if (Vector3.Distance(transform.position, chicken.transform.position) < 10)
                chickenInRange = true;
            else
                chickenInRange = false;
            yield return new WaitForSeconds(0.2f);
        } while (true);
    }

    public void Harvest()
    {
        if (inventoryController.AddDryGrams(inventory))
        {
            inventory = 0;
            isDry = false;
            dryBar.value = 0;
            SetDryBarActive(false);
        }
        else
        {
            Debug.LogWarning("Cannot add amount to inventory.");
        }
    }

    public bool AddToInventory(float _amt)
    {
        bool amtOk = true;

        if (inventory + _amt <= maxInventory)
        {
            inventory += _amt;
        }
        else
        {
            amtOk = false;
            print("Failed to add to dryer inventory");
        }

        return amtOk;
    }

    public void BeginDrying()
    {
        StartCoroutine(DryBarUpdate());
        StartCoroutine(DryRoutine());
        SetDryerPanelActive(false);
        clickActive = true;
    }

    public IEnumerator DryRoutine()
    {
        SetDryBarActive(true);
        yield return new WaitForSeconds(dryTime);
        isDry = true;
    }

    public IEnumerator DryBarUpdate()
    {
        dryBar.value = 0;
        dryBar.maxValue = dryTime + 1;
        for (int i = 0; i < dryTime + 1; i++)
        {
            dryBar.value++;
            yield return new WaitForSeconds(1);
        }
    }

    public void SetDryBarActive(bool _active)
    {
        if (_active)
        {
            dryBarCg.alpha = 1;
            dryBarCg.interactable = true;
            dryBarCg.blocksRaycasts = true;
        }
        else
        {
            dryBarCg.alpha = 0;
            dryBarCg.interactable = false;
            dryBarCg.blocksRaycasts = false;
        }
    }

    public void SetDryerPanelActive(bool _active)
    {
        if (_active)
        {
            dryerPanel.alpha = 1;
            dryerPanel.interactable = true;
            dryerPanel.blocksRaycasts = true;
        }
        else
        {
            dryerPanel.alpha = 0;
            dryerPanel.interactable = false;
            dryerPanel.blocksRaycasts = false;
        }
    }

    public void CancelDryer()
    {
        SetDryerPanelActive(false);
        clickActive = true;
    }

    public void AddWetWeed()
    {
        if(AddToInventory(inventoryController.wetGramsCarrying))
        {
            inventoryController.DropWetGrams();
            isDry = false;
        }
        else
        {
            Debug.LogWarning("Not enough room in Dryer");
        }
        SetDryerPanelActive(false);
        clickActive = true;
    }
}
