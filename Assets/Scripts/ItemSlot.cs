using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IDropHandler
{
    [Header("Settings")]
    public bool acceptsAll;
    public string[] tagsAccepted;

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
            if (CheckTags(_eventData.pointerDrag.tag) || acceptsAll)
            {

                RectTransform dragObjTransform = _eventData.pointerDrag.GetComponent<RectTransform>();
                dragObjTransform.SetParent(transform, false);
                dragObjTransform.anchoredPosition = new Vector2(0, 0);
                _eventData.pointerDrag.GetComponent<InventoryItem>().targetParent = transform;
            }
        }
    }

    private bool CheckTags(string _tag)
    {
        bool acceptable = false;

        for (int i = 0; i < tagsAccepted.Length; i++)
        {
            if (tagsAccepted[i].Equals(_tag))
            {
                acceptable = true;
                break;
            }
        }

        return acceptable;
    }
}
