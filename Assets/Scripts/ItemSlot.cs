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

                // If there is something in the slot
                if (transform.childCount != 0)
                {

                    StrainProfile dragStrainProfile = _eventData.pointerDrag.GetComponent<StrainProfile>();
                    StrainProfile childStrainProfile = transform.GetChild(0).GetComponent<StrainProfile>();

                    if (dragStrainProfile != null)
                    {
                        // Handle checking strains to see if they are the same
                        if (dragStrainProfile.strainID == childStrainProfile.strainID)
                        {
                            CombineItems(dragStrainProfile.GetComponent<InventoryItem>(), childStrainProfile.GetComponent<InventoryItem>());
                            childStrainProfile.GetComponent<InventoryItem>().Lock(true);
                        }
                    }
                    else
                    {
                        // Normal tag check to determine if they are the same
                    }
                }

                // if there is nothing in the slot
                else
                {
                    if (isBuyerSlot)
                    {
                        if (!buyer.orderFilled)
                        {
                            dragItem = _eventData.pointerDrag.GetComponent<InventoryItem>();
                            dragWeedBrick = _eventData.pointerDrag.GetComponent<WeedBrick>();

                            float remainder = buyer.DropOffWeed(dragWeedBrick, dragItem.amount); // change this to send the entire WeedBrick to test if correct
                                                                                                    // start with laptop script, establish means to confirm correct strain


                            // if there is a remainder
                            if (remainder > 0)
                            {
                                float diff = dragItem.amount - remainder;
                                dragItem.SetAmount(remainder);

                                RectTransform dragObjTransform = _eventData.pointerDrag.GetComponent<RectTransform>();
                                dragObjTransform.SetParent(_eventData.pointerDrag.GetComponent<InventoryItem>().previousParent, false);
                                dragObjTransform.anchoredPosition = new Vector2(0, 0);
                                _eventData.pointerDrag.GetComponent<InventoryItem>().targetParent = _eventData.pointerDrag.GetComponent<InventoryItem>().previousParent;


                                GameObject cloneBrick = Instantiate(_eventData.pointerDrag, transform);
                                print("Cloned brick");
                                dragObjTransform = cloneBrick.GetComponent<RectTransform>();
                                dragObjTransform.SetParent(transform, false);
                                dragObjTransform.anchoredPosition = new Vector2(0, 0);
                                

                                childItem = cloneBrick.GetComponent<InventoryItem>();
                                childItem.previousParent = childItem.transform;
                                childItem.SetAmount(diff);
                                childItem.targetParent = transform;
                                childItem.Lock(true);

                            }
                            // if whole brick was returned
                            else if (remainder == dragItem.amount)
                            {
                                dragItem.ReturnToPreviousParent();
                            }
                            // if no brick was returned
                            else if (remainder <= 0)
                            {
                                print("no remainder");
                                RectTransform dragObjTransform = _eventData.pointerDrag.GetComponent<RectTransform>();
                                dragObjTransform.SetParent(transform, false);
                                dragObjTransform.anchoredPosition = new Vector2(0, 0);

                                InventoryItem draggedItem = _eventData.pointerDrag.GetComponent<InventoryItem>();
                                draggedItem.targetParent = transform;
                                draggedItem.previousParent = transform;
                                draggedItem.Lock(true);
                            }
                            
                        }
                    }
                    else
                    {
                        RectTransform dragObjTransform = _eventData.pointerDrag.GetComponent<RectTransform>();
                        dragObjTransform.SetParent(transform, false);
                        dragObjTransform.anchoredPosition = new Vector2(0, 0);
                        _eventData.pointerDrag.GetComponent<InventoryItem>().targetParent = transform;
                        _eventData.pointerDrag.GetComponent<InventoryItem>().Lock(false);
                    }
                }


             


                    //    // OLD SCRIPTS********************
                    //    //if (transform.childCount == 0)
                    //    //{

                    //    //    if (remainder != 0)
                    //    //    {
                    //    //        float diff = dragItem.amount - remainder;
                    //    //        dragItem.AddAmount(-remainder);

                    //    //        RectTransform dragObjTransform = _eventData.pointerDrag.GetComponent<RectTransform>();
                    //    //        dragObjTransform.SetParent(_eventData.pointerDrag.GetComponent<InventoryItem>().previousParent, false);
                    //    //        dragObjTransform.anchoredPosition = new Vector2(0, 0);
                    //    //        _eventData.pointerDrag.GetComponent<InventoryItem>().targetParent = _eventData.pointerDrag.GetComponent<InventoryItem>().previousParent;


                    //    //        GameObject cloneBrick = Instantiate(_eventData.pointerDrag, transform);
                    //    //        dragObjTransform = cloneBrick.GetComponent<RectTransform>();
                    //    //        dragObjTransform.SetParent(transform, false);
                    //    //        dragObjTransform.anchoredPosition = new Vector2(0, 0);

                    //    //        childItem = cloneBrick.GetComponent<InventoryItem>();
                    //    //        childItem.SetAmount(0);
                    //    //        childItem.targetParent = transform;
                    //    //        childItem.AddAmount(diff);
                    //    //    }
                    //    //}
                    //    //else
                    //    //{
                    //    //    if (remainder != 0)
                    //    //    {
                    //    //        float diff = dragItem.amount - remainder;
                    //    //        dragItem.AddAmount(-remainder);

                    //    //        RectTransform dragObjTransform = _eventData.pointerDrag.GetComponent<RectTransform>();
                    //    //        dragObjTransform.SetParent(_eventData.pointerDrag.GetComponent<InventoryItem>().previousParent, false);
                    //    //        dragObjTransform.anchoredPosition = new Vector2(0, 0);
                    //    //        _eventData.pointerDrag.GetComponent<InventoryItem>().targetParent = _eventData.pointerDrag.GetComponent<InventoryItem>().previousParent;


                    //    //        GameObject cloneBrick = transform.GetChild(0).gameObject;

                    //    //        childItem = cloneBrick.GetComponent<InventoryItem>();
                    //    //        childItem.targetParent = transform;
                    //    //        childItem.AddAmount(diff);
                    //    //    }
                    //    //}
                    //}
                    //else
                    //{
                    //    //RectTransform dragObjTransform = _eventData.pointerDrag.GetComponent<RectTransform>();
                    //    //dragObjTransform.SetParent(transform, false);
                    //    //dragObjTransform.anchoredPosition = new Vector2(0, 0);
                    //    //_eventData.pointerDrag.GetComponent<InventoryItem>().targetParent = transform;
                    //}
                }
        }

        //if (transform.childCount != 0)
        //    transform.GetChild(0).GetComponent<InventoryItem>().Lock(true);
    }

    // does not need a buyer strain check because if combining items the child item has already been checked
    // just make sure the child item is checked when dropped in empty slot
    public void CombineItems(InventoryItem _draggedItem, InventoryItem _childItem)
    {
        if (isBuyerSlot)
        {
            if (!buyer.orderFilled)
            {
                if (buyer.amountInInventory + _draggedItem.amount <= buyer.amountRequested)
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
                else
                {
                    _childItem.AddAmount(_draggedItem.amount);
                    _draggedItem.SetAmount(0);

                    float diff = _childItem.amount - buyer.amountRequested;


                    _childItem.AddAmount(-diff);
                    _draggedItem.SetAmount(diff);
                }
            }
            else
            {
                _draggedItem.ReturnToPreviousParent();
            }
        }
        else
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
    }

    public void CombineItems(InventoryItem _draggedItem, InventoryItem _childItem, float _maxAmountToAdd)
    {
        float maxAmt = _childItem.amount + _maxAmountToAdd;
        _childItem.AddAmount(_maxAmountToAdd);
        _draggedItem.AddAmount(-_maxAmountToAdd);

        float diff = _childItem.amount - maxAmt;

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
