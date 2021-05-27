using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CompareSlot : MonoBehaviour, IDropHandler
{
    private StrainInfoUI strainInfoUI;
    private InventoryController inventoryController;

    public ComparePanel comparePanel;
    public bool slotFilled;
    public StrainProfile slotStrain;
    public Transform previousSlot;

    private void Start()
    {
        if (!strainInfoUI)
            strainInfoUI = StrainInfoUI.instance.strainInfoUI.GetComponent<StrainInfoUI>();

        if (!inventoryController)
            inventoryController = InventoryController.instance.inventoryController.GetComponent<InventoryController>();

        StartCoroutine(SlotCheck());
    }

    private IEnumerator SlotCheck()
    {
        do
        {
            if (transform.childCount != 0)
            {
                if (!slotFilled)
                {
                    slotFilled = true;
                    slotStrain = transform.GetChild(0).GetComponent<StrainProfile>();
                    comparePanel.SetComparePanelActive(strainInfoUI.currStrain, slotStrain);
                    slotStrain.transform.SetParent(inventoryController.GetOpenSlot(), false);
                    slotStrain.transform.position = slotStrain.transform.parent.position;
                }
            }
            else
                slotFilled = false;

            yield return new WaitForSeconds(0.2f);
        } while (true);
    }

    public void OnDrop(PointerEventData _eventData)
    {
        if (_eventData.pointerDrag != null)
        {
            previousSlot = _eventData.pointerDrag.transform.parent;
            RectTransform dragObjTransform = _eventData.pointerDrag.GetComponent<RectTransform>();
            dragObjTransform.SetParent(transform, false);
            dragObjTransform.anchoredPosition = new Vector2(0, 0);
            _eventData.pointerDrag.GetComponent<InventoryItem>().targetParent = transform;
        }
    }

    public void ClearSlot()
    {
        
    }
}
