using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DryerController : MonoBehaviour
{
    [Header("Settings")]
    public float dryTime;
    public bool clickActive;
    public bool chickenInRange;

    [Header("Status")]
    public bool placed;

    [Header("Setup")]
    public GameObject colliderObj;
    public Button[] btnsToDisable;
    public Slider dryBarWorld;
    public Slider dryBarUI;
    public CanvasGroup dryBarCg;
    public CanvasGroup dryerPanel;

    [Header("Inventory Slots")]
    public Transform slotsParent;
    public List<Transform> slots;

    private ChickenController chicken;
    private InventoryController inventoryController;


    private void Start()
    {
        if (!inventoryController)
            inventoryController = InventoryController.instance.inventoryController.GetComponent<InventoryController>();

        if (!chicken)
            chicken = ChickenController.instance.chickenController.GetComponent<ChickenController>();

        if (slots.Count == 0)
        {
            for (int i = 0; i < slotsParent.childCount; i++)
            {
                slots.Add(slotsParent.GetChild(i));
            }
        }
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

    IEnumerator InteractCheck()
    {
        do
        {
            if (Input.GetKey("f"))
            {
                SetDryerPanelActive(true);
            }
            yield return new WaitForSeconds(0.1f);
        } while (chickenInRange);
    }

    public void SetInteractable(bool _canInteract)
    {
        if (_canInteract)
        {
            if (!chickenInRange)
            {
                chickenInRange = true;
                StartCoroutine(InteractCheck());
            }

        }
        else
        {
            if (chickenInRange)
            {
                chickenInRange = false;
            }
        }
    }

    public void PlaceDryer()
    {
        StartCoroutine(PlaceRoutine());
        colliderObj.tag = "Dryer";

    }

    IEnumerator PlaceRoutine()
    {
        yield return new WaitForSeconds(1);
        placed = true;

    }

    public void BeginDrying()
    {
        StartCoroutine(DryBarUpdate());
        StartCoroutine(DryRoutine());
        SetDryerPanelActive(false);
        BtnsEnabled(false);
        clickActive = true;
    }

    private void BtnsEnabled(bool _active)
    {
        if (_active)
        {
            for (int i = 0; i < btnsToDisable.Length; i++)
            {
                btnsToDisable[i].interactable = true;
            }
        }
        else
        {
            for (int i = 0; i < btnsToDisable.Length; i++)
            {
                btnsToDisable[i].interactable = false;
            }
        }
    }

    public IEnumerator DryRoutine()
    {
        SetDryBarActive(true);
        yield return new WaitForSeconds(dryTime);
        
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].childCount != 0)
            {
                slots[i].GetChild(0).GetComponent<WeedBrick>().SetDry(true);
            }
        }
        BtnsEnabled(true);
        
    }

    public IEnumerator DryBarUpdate()
    {
        dryBarWorld.value = 0;
        dryBarUI.value = 0;

        dryBarWorld.maxValue = dryTime + 1;
        dryBarUI.maxValue = dryTime + 1;

        for (int i = 0; i < dryTime + 1; i++)
        {
            dryBarWorld.value++;
            dryBarUI.value++;
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

}
