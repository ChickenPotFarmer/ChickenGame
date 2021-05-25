using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IDropHandler
{
    [Header("Settings")]
    public bool isBuyerSlot;
    public bool acceptsAll;
    public string[] tagsAccepted;

    private RectTransform rectTransform;
    private Buyer buyer;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        if (isBuyerSlot && !buyer)
            buyer = GetComponentInParent<Buyer>();
    }
    public void OnDrop (PointerEventData _eventData)
    {
        Debug.Log("OnDrop");
        if (_eventData.pointerDrag != null)
        {
            if (CheckTags(_eventData.pointerDrag.tag) || acceptsAll)
            {
                // check to see if slot has something in it
                // if it does, check to see what kind of item it is
                // if it does, check to see if the icon is full
                // if not, return itemMaxAmt - currentAmt, max slot out


                if (isBuyerSlot)
                {
                    WeedBrick weedBrick = _eventData.pointerDrag.GetComponent<WeedBrick>();

                    float remainder = buyer.DropOffWeed(weedBrick.grams); // change this to send the entire WeedBrick to test if correct
                                                                            // start with laptop script, establish means to confirm correct strain
                                                                            // move to buyer script, adjust weeddropoff method so it takes in full plant and can compare them
                                                                            // add UI function so items can be stacked and split
                    if (remainder != 0)
                    {
                        float diff = weedBrick.grams - remainder;
                        weedBrick.grams = remainder;

                        RectTransform dragObjTransform = _eventData.pointerDrag.GetComponent<RectTransform>();
                        dragObjTransform.SetParent(_eventData.pointerDrag.GetComponent<InventoryItem>().previousParent, false);
                        dragObjTransform.anchoredPosition = new Vector2(0, 0);
                        _eventData.pointerDrag.GetComponent<InventoryItem>().targetParent = _eventData.pointerDrag.GetComponent<InventoryItem>().previousParent;


                        GameObject cloneBrick = Instantiate(_eventData.pointerDrag, transform);
                        dragObjTransform = cloneBrick.GetComponent<RectTransform>();
                        dragObjTransform.SetParent(transform, false);
                        dragObjTransform.anchoredPosition = new Vector2(0, 0);
                        cloneBrick.GetComponent<InventoryItem>().targetParent = transform;
                        weedBrick = cloneBrick.GetComponent<WeedBrick>();
                        weedBrick.grams = diff;
                    }
                }
                else
                {
                    RectTransform dragObjTransform = _eventData.pointerDrag.GetComponent<RectTransform>();
                    dragObjTransform.SetParent(transform, false);
                    dragObjTransform.anchoredPosition = new Vector2(0, 0);
                    _eventData.pointerDrag.GetComponent<InventoryItem>().targetParent = transform;
                }
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
