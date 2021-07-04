using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageCrate : MonoBehaviour
{
    [Header("Status")]
    public bool placed;

    [Header("Setup")]
    public Transform slotsParent;
    public CanvasGroup cg;
    public Animator animator;
    public List<Transform> slots;

    private Transform openSlot;
    private InventoryController inventoryController;
    private SmartDropdown smartDropdown;
    private void Start()
    {
        if (!inventoryController)
            inventoryController = InventoryController.instance.inventoryController.GetComponent<InventoryController>();

        if (!smartDropdown)
            smartDropdown = SmartDropdown.instance.smartDropdown.GetComponent<SmartDropdown>();

        IntializeSlots();
    }

    public void OpenCrate(bool _open)
    {
        if (_open)
        {
            StartCoroutine(OpenCrateRoutine(true));
        }
        else
        {
            StartCoroutine(OpenCrateRoutine(false));
        }
    }

    public void CloseCrate()
    {
        StartCoroutine(OpenCrateRoutine(false));
    }

    public IEnumerator OpenCrateRoutine(bool _open)
    {
        if (_open)
        {
            animator.Play("Open");
            yield return new WaitForSeconds(1);
            SetPanelActive(true);
        }
        else
        {
            SetPanelActive(false);
            yield return new WaitForSeconds(0.05f);
            animator.Play("Close");


        }
    }

    public void SetPanelActive(bool _active)
    {
        if (_active)
        {
            cg.alpha = 1;
            cg.interactable = true;
            cg.blocksRaycasts = true;
            smartDropdown.SetStorageDropdown(slotsParent);
        }
        else
        {
            cg.alpha = 0;
            cg.interactable = false;
            cg.blocksRaycasts = false;
            smartDropdown.UnsetStorage();
        }
    }

    public void PlaceCrate()
    {
        StartCoroutine(PlaceCrateRoutine());
    }

    IEnumerator PlaceCrateRoutine()
    {
        yield return new WaitForSeconds(1);
        placed = true;

    }

    private void IntializeSlots()
    {
        // Intialize Slots References
        if (slots.Count == 0)
        {
            for (int i = 0; i < slotsParent.childCount; i++)
            {
                slots.Add(slotsParent.GetChild(i));
            }
        }
    }

    public void ClearInventory()
    {
        inventoryController.ReturnAllItems(slotsParent);
    }

    public Transform GetOpenSlot()
    {
        openSlot = null;

        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].childCount == 0)
            {
                openSlot = slots[i];
                break;
            }
        }

        return openSlot;

    }

    public InventoryItem GetItemToCombine(InventoryItem _itemToCombine)
    {
        InventoryItem inventoryItem;
        InventoryItem itemToCombine = null;
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].childCount != 0)
            {
                inventoryItem = slots[i].GetChild(0).GetComponent<InventoryItem>();
                if (_itemToCombine.itemID == inventoryItem.itemID && inventoryItem.amount < inventoryItem.maxAmount && _itemToCombine.CompareTag(inventoryItem.tag))
                {
                    openSlot = slots[i];
                    itemToCombine = inventoryItem;
                    break;
                }
            }
        }

        return itemToCombine;
    }

    public InventoryItem CombineItems(InventoryItem _draggedItem, InventoryItem _childItem)
    {
        InventoryItem remainderItem = null;
        _childItem.AddAmount(_draggedItem.amount);
        _draggedItem.amount = 0;

        float diff = _childItem.amount - _childItem.maxAmount;

        //if there is remainder
        if (diff > 0)
        {
            _childItem.AddAmount(-diff);
            _draggedItem.SetAmount(diff);


            remainderItem = _draggedItem;
            _draggedItem.ReturnToPreviousParent();
        }
        else
        {
            Destroy(_draggedItem.gameObject);
        }

        return remainderItem;

    }

    public InventoryItem ReturnToStorage(InventoryItem _item)
    {
        // check for stack to combine with
        // if there is, combine stacks
        // if there is remainder, spawn new stack, call ReturnToInventory on new stack

        Transform openSlot = null;
        InventoryItem itemToCombine = GetItemToCombine(_item);
        InventoryItem remainderItem = null;
        float itemAmt;

        // Check for stack to combine with
        if (itemToCombine != null)
        {
            itemAmt = _item.amount;
            // if there is, combine stacks.
            remainderItem = CombineItems(_item, itemToCombine);

            // I think this will make it stop trying to store them if it's the same amount coming out as going in.
            // needs testing
            if (remainderItem != null && remainderItem.amount == itemAmt)
            {
                print("inventory full? I think?");

                // on a hunch I added this, needs testing
                if (remainderItem.amount == 0)
                    remainderItem = null;
            }
            // If there is a remainder, call ReturnToInventory on it
            else if (remainderItem != null && remainderItem.amount > 0)
            {
                ReturnToStorage(remainderItem);
            }
        }
        else
        {
            openSlot = GetOpenSlot();

            if (openSlot != null)
            {
                _item.transform.SetParent(openSlot, false);
                _item.transform.position = _item.transform.parent.position;
                _item.Lock(false);
            }
            else
            {
                print("INVENTORY FULL");
                remainderItem = _item;
            }

        }

        if (_item)
            _item.UpdateCurrentParent();

        if (remainderItem)
            remainderItem.UpdateCurrentParent();

        return remainderItem;
    }

    public void ReturnAllItems(Transform _slotsParent)
    {
        InventoryItem remainderItem;
        Transform[] tempSlots = new Transform[_slotsParent.childCount];

        for (int i = 0; i < _slotsParent.childCount; i++)
        {
            tempSlots[i] = _slotsParent.GetChild(i);
        }

        List<InventoryItem> slotItems = new List<InventoryItem>();

        for (int i = 0; i < tempSlots.Length; i++)
        {
            if (tempSlots[i].childCount != 0)
            {
                slotItems.Add(tempSlots[i].GetChild(0).GetComponent<InventoryItem>());
            }
        }

        for (int i = 0; i < slotItems.Count; i++)
        {
            remainderItem = ReturnToStorage(slotItems[i]);

            if (remainderItem != null && remainderItem.amount > 0)
            {
                Debug.LogWarning("INVENTORY FULL");
            }

        }
    }

    public void ReturnAllItems(Transform _slotsParent, string _itemId)
    {
        InventoryItem remainderItem;
        Transform[] tempSlots = new Transform[_slotsParent.childCount];

        for (int i = 0; i < _slotsParent.childCount; i++)
        {
            tempSlots[i] = _slotsParent.GetChild(i);
        }

        List<InventoryItem> slotItems = new List<InventoryItem>();

        for (int i = 0; i < tempSlots.Length; i++)
        {
            if (tempSlots[i].childCount != 0)
            {
                if (tempSlots[i].GetChild(0).GetComponent<InventoryItem>().itemID == _itemId) // refactor
                    slotItems.Add(tempSlots[i].GetChild(0).GetComponent<InventoryItem>());
            }
        }

        for (int i = 0; i < slotItems.Count; i++)
        {
            remainderItem = ReturnToStorage(slotItems[i]);

            if (remainderItem != null && remainderItem.amount > 0)
            {
                Debug.LogWarning("INVENTORY FULL");
            }

        }
    }
}
