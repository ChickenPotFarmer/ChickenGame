using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IDropHandler
{
    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    public void OnDrop (PointerEventData _eventData)
    {
        Debug.Log("OnDrop");
        if (_eventData.pointerDrag != null)
        {
            RectTransform dragObjTransform = _eventData.pointerDrag.GetComponent<RectTransform>();
            dragObjTransform.SetParent(transform, false);
            dragObjTransform.anchoredPosition = new Vector2(0, 0);
            _eventData.pointerDrag.GetComponent<InventoryItem>().targetParent = transform;
        }
    }
}
