using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryItem : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler
{
    [Header("Item Info")]
    public string itemName;
    public float amount;
    public float maxAmount;
    public bool isStrain;
    public bool locked;
    public bool inPlayerInventory;
    public float storeCost;
    public string itemID;

    [Header("Drag Info")]
    public Transform previousParent;
    public Transform targetParent;
    public Transform currentParent;

    [Header("UI Text")]
    public Text uiName;
    public Text uiAmount;

    private CanvasGroup cg;
    private Canvas inventoryCanvas;
    private RectTransform rectTransform;
    private HoverInfo hoverInfo;
    public StrainProfile strainProfile;
    //private SeedDropDown seedDropDown;
    private InventoryController inventoryController;
    private SmartDropdown smartDropdown;


    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        cg = GetComponent<CanvasGroup>();
    }

    void Start()
    {
        if (!hoverInfo)
            hoverInfo = HoverInfo.instance.hoverInfo.GetComponent<HoverInfo>();

        if (isStrain && !strainProfile)
            strainProfile = GetComponent<StrainProfile>();

        if (!smartDropdown)
            smartDropdown = SmartDropdown.instance.smartDropdown.GetComponent<SmartDropdown>();

        if (!inventoryController)
            inventoryController = InventoryController.instance.inventoryController.GetComponent<InventoryController>();

        inventoryCanvas = inventoryController.inventoryCanvas;

        IntializeItem();
    }

    private void IntializeItem()
    {
        if (currentParent != null)
        {
            SetNewParent(currentParent);
        }
        else
        {
            UpdateCurrentParent();
        }
        

        transform.localScale = new Vector3(1, 1, 1);

        SetItemName();

        UpdateTextUI();
    }

    public void OnBeginDrag(PointerEventData _EventData)
    {
        previousParent = transform.parent;
        rectTransform.SetParent(inventoryController.dragParent, true);
        cg.blocksRaycasts = false;
        cg.alpha = 0.7f;
        smartDropdown.CloseAndResetDropdown();
    }

    public void OnDrag(PointerEventData _EventData)
    {
        rectTransform.anchoredPosition += _EventData.delta / inventoryCanvas.scaleFactor;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.CompareTag("Dryer Pile"))
                {
                    print("weed dropped on Dryer");
                    targetParent = hit.collider.transform;
                }
                else if (hit.collider.gameObject.CompareTag("Weed Plant"))
                {
                    targetParent = hit.collider.transform;
                }
                else
                {
                    targetParent = null;
                }
            }
        }

    }

    // Handles cases where item isn't dropped on Inventory Slot
    public void OnEndDrag(PointerEventData _EventData)
    {
        if (!locked)
        {
            cg.blocksRaycasts = true;
            cg.alpha = 1;
        }

            // Checks if it was dropped, if not then return to original slot.
        if (targetParent == null)
        {
            if (transform.parent == inventoryController.dragParent)
            {
                ReturnToPreviousParent();
            }
        }
        UpdateCurrentParent();
        inventoryController.UpdateDecoChicks();

    }

    private void CheckIfInPlayerInventory()
    {
        if (currentParent != null)
        {
            if (currentParent.GetComponent<ItemSlot>().isPlayerSlot)
            {
                inPlayerInventory = true;
            }
            else
            {
                inPlayerInventory = false;
            }
        }
    }

    public void UpdateCurrentParent()
    {
        currentParent = transform.parent;
        CheckIfInPlayerInventory();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // find better way to do this
        //if (isStrain)
        //{
        //    seedDropDown.SetDropdownActive(true);
        //    seedDropDown.SetStrainInfoBtn(GetComponent<StrainProfile>());
        //}
        print("inventory item clicks");
        smartDropdown.OpenDropdown(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        string[] info = new string[4];
        if (isStrain)
        {
            info[0] = strainProfile.strainName;
            info[1] = strainProfile.GetStrainType();
            info[2] = "THC: " + strainProfile.GetReaderFriendlyThcContent();
            info[3] = "Terpene: " + (strainProfile.totalTerpenesPercent * 100).ToString("n2") + "%";
        }
        else
        {
            info[0] = itemName;
            info[1] = "NULL";
            info[2] = "NULL";
            info[3] = "NULL";
        }

        hoverInfo.SetText(info);


        hoverInfo.SetHoverActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hoverInfo.SetHoverActive(false);

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        print("OnPointerUp");
    }

    public void ReturnToPreviousParent()
    {
        targetParent = previousParent;
        rectTransform.SetParent(previousParent, false);
        rectTransform.anchoredPosition = new Vector2(0, 0);
    }

    public void SetNewParent(Transform _parent)
    {
        targetParent = _parent;
        rectTransform.SetParent(_parent, false);
        rectTransform.anchoredPosition = new Vector2(0, 0);
        UpdateCurrentParent();
    }

    public void Lock(bool _locked)
    {
        if (_locked)
        {
            locked = true;
            cg.blocksRaycasts = false;
            cg.alpha = 0.7f;
        }   
        else
        {
            locked = false;
            cg.blocksRaycasts = true;
            cg.alpha = 1f;
        }

    }

    public bool AddAmount(float _amt)
    {
        bool itemDestroyed = false;
        amount += _amt;

        if (amount <= 0)
        {
            itemDestroyed = true;
        }

        UpdateTextUI();
        if (itemDestroyed && gameObject != null)
            Destroy(gameObject, 0.05f);
        return itemDestroyed;
    }

    public bool SetAmount(float _amt)
    {
        bool itemDestroyed = false;
        amount = _amt;

        if (amount <= 0)
        {
            itemDestroyed = true;
        }

        UpdateTextUI();
        if (itemDestroyed)
            Destroy(gameObject, 0.05f);
        return itemDestroyed;
    }

    public void UpdateTextUI()
    {
        if (uiAmount != null)
        {
            if (itemName.Equals("Cash"))
                uiAmount.text = "$" + amount.ToString();
            else
                uiAmount.text = amount.ToString();
        }
        if (uiName != null)
            uiName.text = itemName;

    }

    public void SetItemName()
    {
        if (itemName == "")
        {
            StrainProfile strain = GetComponent<StrainProfile>();

            if (strain)
            {
                itemName = strain.strainName;
                uiName.text = itemName;

            }
            else
                print("no strainprofile found");
        }
        else
        {
            if (!itemName.Equals("Cash"))
                uiName.text = itemName;
        }
    }
}
