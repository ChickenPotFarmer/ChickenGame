using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryItem : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler
{
    [Header("Item Info")]
    public bool isSeed;

    [Header("Drag Info")]
    public Transform previousParent;
    public Transform targetParent;
    private CanvasGroup cg;

    private Canvas inventoryCanvas;
    private RectTransform rectTransform;
    private InventoryGUI inventoryGUI;
    private HoverInfo hoverInfo;
    private StrainProfile strainProfile;
    private SeedDropDown seedDropDown;

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

        inventoryCanvas = inventoryGUI.inventoryCanvas;
    }

    public void OnBeginDrag(PointerEventData _EventData)
    {
        Debug.Log("OnBeginDrag");
        previousParent = transform.parent;
        rectTransform.SetParent(inventoryGUI.dragParent, true);
        cg.blocksRaycasts = false;
        cg.alpha = 0.7f;
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
                else
                {
                    targetParent = null;
                }
            }
        }

    }

    public void OnEndDrag(PointerEventData _EventData)
    {
        Debug.Log("OnEndDrag");

        cg.blocksRaycasts = true;
        cg.alpha = 1;

        // Checks if it was dropped, if not then return to original slot.
        if (targetParent == null)
        {
            if (transform.parent == inventoryGUI.dragParent)
            {
                rectTransform.SetParent(previousParent, false);
                rectTransform.anchoredPosition = new Vector2(0, 0);
            }
        }
    }

    public void EndDragRoutine()
    {
        cg.blocksRaycasts = true;
        cg.alpha = 1;

        // Checks if it was dropped, if not then return to original slot.
        if (transform.parent == inventoryGUI.dragParent)
        {
            rectTransform.SetParent(previousParent, false);
            rectTransform.anchoredPosition = new Vector2(0, 0);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isSeed)
            seedDropDown.SetDropdownActive(true);
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
}
