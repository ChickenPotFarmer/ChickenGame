using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryItem : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler
{
    [Header("Item Info")]
    public float amount;
    public float maxAmount;
    public bool isSeed;
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

        if (isSeed && !strainProfile)
            strainProfile = GetComponent<StrainProfile>();

        if (!seedDropDown)
            seedDropDown = SeedDropDown.instance.seedDropDown.GetComponent<SeedDropDown>();

        if (!inventoryController)
            inventoryController = InventoryController.instance.inventoryController.GetComponent<InventoryController>();

        inventoryCanvas = inventoryGUI.inventoryCanvas;
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
        StrainProfile dragStrain;
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
        if (isSeed)
        {
            seedDropDown.SetDropdownActive(true);
            seedDropDown.SetStrainInfoBtn(GetComponent<StrainProfile>());
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isSeed)
            hoverInfo.SetText(strainProfile.strainName + " Seeds");

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
}
