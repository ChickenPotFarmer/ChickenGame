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
    private InventoryItem dragItem;
    private InventoryItem childItem;
    private WeedBrick dragWeedBrick;

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

                if (transform.childCount != 0)
                {

                    StrainProfile dragStrainProfile = _eventData.pointerDrag.GetComponent<StrainProfile>();
                    StrainProfile childStrainProfile = transform.GetChild(0).GetComponent<StrainProfile>();

                    if (dragStrainProfile != null)
                    {
                        // Handle checking strains to see if they are the same
                        if (dragStrainProfile.strainID == childStrainProfile.strainID)
                            CombineItems(dragStrainProfile.GetComponent<InventoryItem>(), childStrainProfile.GetComponent<InventoryItem>());
                    }
                    else
                    {
                        // Normal tag check to determine if they are the same
                    }
                }
                else
                {
                    RectTransform dragObjTransform = _eventData.pointerDrag.GetComponent<RectTransform>();
                    dragObjTransform.SetParent(transform, false);
                    dragObjTransform.anchoredPosition = new Vector2(0, 0);
                    _eventData.pointerDrag.GetComponent<InventoryItem>().targetParent = transform;
                }


                if (isBuyerSlot)
                {
                    dragItem = _eventData.pointerDrag.GetComponent<InventoryItem>();
                    dragWeedBrick = _eventData.pointerDrag.GetComponent<WeedBrick>();

                    if (transform.childCount == 0)
                    {
                        



                        float remainder = buyer.DropOffWeed(dragWeedBrick, dragItem.amount); // change this to send the entire WeedBrick to test if correct
                                                                                             // start with laptop script, establish means to confirm correct strain


                        // if there is a remainder
                        if (remainder != 0)
                        {
                            // calc how much buyer took (buyerWeedBrick)
                            // remainder is how much should be returned (inventoryWeedBrick) 

                            // spawn in new weed brick for the buyer
                            // set buyer weedbrick to the amount the buyer took

                            // set inventoryWeedBrick to remainder
                            // return brick to inventory


                        }
                        // if whole brick was returned
                        else if (remainder == dragItem.amount)
                        {
                            dragItem.targetParent = dragItem.previousParent;
                        }
                        // if no brick was returned
                        else if (remainder == 0)
                        {

                        }
                    }


                    // OLD SCRIPTS********************
                    //if (transform.childCount == 0)
                    //{

                    //    if (remainder != 0)
                    //    {
                    //        float diff = dragItem.amount - remainder;
                    //        dragItem.AddAmount(-remainder);

                    //        RectTransform dragObjTransform = _eventData.pointerDrag.GetComponent<RectTransform>();
                    //        dragObjTransform.SetParent(_eventData.pointerDrag.GetComponent<InventoryItem>().previousParent, false);
                    //        dragObjTransform.anchoredPosition = new Vector2(0, 0);
                    //        _eventData.pointerDrag.GetComponent<InventoryItem>().targetParent = _eventData.pointerDrag.GetComponent<InventoryItem>().previousParent;


                    //        GameObject cloneBrick = Instantiate(_eventData.pointerDrag, transform);
                    //        dragObjTransform = cloneBrick.GetComponent<RectTransform>();
                    //        dragObjTransform.SetParent(transform, false);
                    //        dragObjTransform.anchoredPosition = new Vector2(0, 0);

                    //        childItem = cloneBrick.GetComponent<InventoryItem>();
                    //        childItem.SetAmount(0);
                    //        childItem.targetParent = transform;
                    //        childItem.AddAmount(diff);
                    //    }
                    //}
                    //else
                    //{
                    //    if (remainder != 0)
                    //    {
                    //        float diff = dragItem.amount - remainder;
                    //        dragItem.AddAmount(-remainder);

                    //        RectTransform dragObjTransform = _eventData.pointerDrag.GetComponent<RectTransform>();
                    //        dragObjTransform.SetParent(_eventData.pointerDrag.GetComponent<InventoryItem>().previousParent, false);
                    //        dragObjTransform.anchoredPosition = new Vector2(0, 0);
                    //        _eventData.pointerDrag.GetComponent<InventoryItem>().targetParent = _eventData.pointerDrag.GetComponent<InventoryItem>().previousParent;


                    //        GameObject cloneBrick = transform.GetChild(0).gameObject;

                    //        childItem = cloneBrick.GetComponent<InventoryItem>();
                    //        childItem.targetParent = transform;
                    //        childItem.AddAmount(diff);
                    //    }
                    //}
                }
                else
                {
                    //RectTransform dragObjTransform = _eventData.pointerDrag.GetComponent<RectTransform>();
                    //dragObjTransform.SetParent(transform, false);
                    //dragObjTransform.anchoredPosition = new Vector2(0, 0);
                    //_eventData.pointerDrag.GetComponent<InventoryItem>().targetParent = transform;
                }
            }
        }
    }

    public void CombineItems(InventoryItem _draggedItem, InventoryItem _childItem)
    {
        _childItem.AddAmount(_draggedItem.amount);
        _draggedItem.SetAmount(0);

        float diff = _childItem.amount - _childItem.maxAmount;

        //if there is remainder
        if (diff > 0)
        {
            _childItem.AddAmount(-diff);
            _draggedItem.SetAmount(diff);
            _draggedItem.ReturnToPreviousParent();

        }
        else
        {
            Destroy(_draggedItem.gameObject);
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
