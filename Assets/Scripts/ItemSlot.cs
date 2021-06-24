using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IDropHandler
{
    [Header("Outside Script To Fire On Drop")]
    public SeedCannon seedCannon;
    public PlanterChickHub planterChickHub;


    [Header("Status")]
    public bool slotFull;

    [Header("Settings")]
    public bool lockOnDrop;
    public bool isPlayerSlot;
    public bool isBuyerSlot;
    public bool acceptsAll;
    public bool acceptsNone;
    public string[] tagsAccepted;

    private RectTransform rectTransform;
    private Buyer buyer;
    private InventoryItem dragItem;
    private InventoryItem childItem;
    private StrainProfile dragWeedstrain;
    private InventoryController inventoryController;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        if (isBuyerSlot && !buyer)
            buyer = GetComponentInParent<Buyer>();

        if (!inventoryController)
            inventoryController = InventoryController.instance.inventoryController.GetComponent<InventoryController>();
    }
    public void OnDrop (PointerEventData _eventData)
    {
        if (!acceptsNone && _eventData.pointerDrag != null)
        {
            InventoryItem dragInventoryItem = _eventData.pointerDrag.GetComponent<InventoryItem>();

            if (CheckTags(_eventData.pointerDrag.tag) || acceptsAll)
            {
                // check to see if slot has something in it
                // if it does, check to see what kind of item it is
                // if it does, check to see if the icon is full
                // if not, return itemMaxAmt - currentAmt, max slot out

                // If there is something in the slot
                if (transform.childCount != 0)
                {

                    InventoryItem childInventoryItem = transform.GetChild(0).GetComponent<InventoryItem>();


                    // Handle checking strains to see if they are the same
                    if (dragInventoryItem.itemID == childInventoryItem.itemID)
                    {
                        CombineItems(dragInventoryItem, childInventoryItem);
                    }
                    
                }

                // if there is nothing in the slot
                else
                {
                    if (isBuyerSlot && !buyer.orderFilled)
                    { 
                        dragItem = dragInventoryItem;
                        dragWeedstrain = _eventData.pointerDrag.GetComponent<StrainProfile>();

                        if (buyer.PassesInspection(dragWeedstrain)) // TO-DO: FIX INSPECTION
                        {

                            float remainder = buyer.DropOffWeed(dragItem.amount); // change this to send the entire WeedBrick to test if correct
                                                                                                    // start with laptop script, establish means to confirm correct strain


                            // if there is a remainder
                            if (remainder > 0)
                            {
                                float diff = dragItem.amount - remainder;
                                dragItem.SetAmount(remainder);

                                RectTransform dragObjTransform = _eventData.pointerDrag.GetComponent<RectTransform>();
                                dragObjTransform.SetParent(dragInventoryItem.previousParent, false);
                                dragObjTransform.anchoredPosition = new Vector2(0, 0);
                                dragInventoryItem.targetParent = dragInventoryItem.previousParent;


                                GameObject cloneBrick = Instantiate(_eventData.pointerDrag, transform);
                                print("Cloned brick");
                                dragObjTransform = cloneBrick.GetComponent<RectTransform>();
                                dragObjTransform.SetParent(transform, false);
                                dragObjTransform.anchoredPosition = new Vector2(0, 0);


                                childItem = cloneBrick.GetComponent<InventoryItem>();
                                childItem.previousParent = childItem.transform;
                                childItem.SetAmount(diff);
                                childItem.SetNewParent(transform);
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

                                dragInventoryItem.targetParent = transform;
                                dragInventoryItem.previousParent = transform;
                                dragInventoryItem.Lock(true);
                            }
                        }
                        
                    }
                    else
                    {
                        RectTransform dragObjTransform = _eventData.pointerDrag.GetComponent<RectTransform>();
                        dragObjTransform.SetParent(transform, false);
                        dragObjTransform.anchoredPosition = new Vector2(0, 0);
                        dragInventoryItem.targetParent = transform;
                        dragInventoryItem.Lock(false);
                    }
                }
            }
            
            TriggerOutsideOnDrops(dragInventoryItem.amount);

            if (lockOnDrop)
                dragInventoryItem.Lock(true);


            // TO-DO: Set up way to detect this if not being dropped on.
            if (HasItem())
                slotFull = true;
            else
                slotFull = false;
        }

        inventoryController.UpdateDecoChicks();

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
                    _draggedItem.amount = 0;

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
                    _draggedItem.amount = 0;

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
            _draggedItem.amount = 0;

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
        _draggedItem.amount -= _maxAmountToAdd;

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

    public void TriggerOutsideOnDrops(float _amt)
    {
        if (seedCannon)
            seedCannon.OnItemDrop();
        if (planterChickHub)
            planterChickHub.OnSeedItemDrop(_amt);
    }

    public bool HasItem()
    {
        if (transform.childCount != 0)
            return true;
        else
            return false;
    }

    public GameObject GetItem()
    {
        if (transform.childCount != 0)
            return transform.GetChild(0).gameObject;
        else
            return null;
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
