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
    public string itemID;

    [Header("Drag Info")]
    public Transform previousParent;
    public Transform targetParent;

    [Header("UI Text")]
    public Text uiName;
    public Text uiAmount;

    private CanvasGroup cg;
    private Canvas inventoryCanvas;
    private RectTransform rectTransform;
    private InventoryGUI inventoryGUI;
    private HoverInfo hoverInfo;
    private StrainProfile strainProfile;
    private SeedDropDown seedDropDown;
    private InventoryController inventoryController;


    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        cg = GetComponent<CanvasGroup>();
    }

    void Start()
    {
        if (!inventoryGUI)
            inventoryGUI = InventoryGUI.instance.inventoryGUI.GetComponent<InventoryGUI>();

        if (!hoverInfo)
            hoverInfo = HoverInfo.instance.hoverInfo.GetComponent<HoverInfo>();

        if (isStrain && !strainProfile)
            strainProfile = GetComponent<StrainProfile>();

        if (!seedDropDown)
            seedDropDown = SeedDropDown.instance.seedDropDown.GetComponent<SeedDropDown>();

        if (!inventoryController)
            inventoryController = InventoryController.instance.inventoryController.GetComponent<InventoryController>();

        inventoryCanvas = inventoryGUI.inventoryCanvas;
        SetItemName();
        UpdateTextUI();
    }

    public void OnBeginDrag(PointerEventData _EventData)
    {
        previousParent = transform.parent;
        rectTransform.SetParent(inventoryGUI.dragParent, true);
        cg.blocksRaycasts = false;
        cg.alpha = 0.7f;
        seedDropDown.SetDropdownActive(false);
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
            if (transform.parent == inventoryGUI.dragParent)
            {
                ReturnToPreviousParent();
            }
        }

        inventoryController.UpdateDecoChicks();

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // find better way to do this
        if (isStrain)
        {
            seedDropDown.SetDropdownActive(true);
            seedDropDown.SetStrainInfoBtn(GetComponent<StrainProfile>());
        }


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
            Destroy(gameObject);
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
            Destroy(gameObject, 0.5f);
        return itemDestroyed;
    }

    public void UpdateTextUI()
    {
        if (uiAmount != null)
            uiAmount.text = amount.ToString();
    }

    public void SetItemName()
    {
        if (itemName == "")
        {
            StrainProfile strain = GetComponent<StrainProfile>();

            if (strain)
                uiName.text = strain.strainName;
        }
        else
        {
            uiName.text = itemName;
        }
    }
}
