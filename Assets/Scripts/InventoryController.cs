using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{
    [Header("Money")]
    public float moneyCarrying;
    public Text moneyTxt;

    [Header("Inventory Chicks")]
    public GameObject chickPrefab;
    public Transform chicksParent;
    public List<ChickInventory> chicks;

    [Header("Inventory Slots")]
    public Transform slotsParent;
    public List<Transform> slots;

    [Header("Settings")]
    public float gramsPerBrick;
    public GameObject slotPrefab;


    public static InventoryController instance;
    [HideInInspector]
    public GameObject inventoryController;

    private Transform openSlot;

    private void Awake()
    {
        instance = this;
        inventoryController = gameObject;
    }

    // TESTING
    private void Update()
    {
        if (Input.GetKeyDown("n"))
            AddInventoryChick();
    }

    private void Start()
    {
        AddCash(0);
        // Intialize Slots References
        if (slots.Count == 0)
        {
            for (int i = 0; i < slotsParent.childCount; i++)
            {
                slots.Add(slotsParent.GetChild(i));
            }
        }
    }

    // TO-DO:
    // Change this so that money is an object in the game
    // player will need to deposit money at bank or atm to buy stuff online
    public void AddCash(float _amt)
    {
        moneyCarrying += _amt;

        moneyTxt.text = "$" + moneyCarrying.ToString("n2");
    }

    public void AddInventoryChick()
    {
        // Add and setup new ChickInventory
        GameObject newChick = Instantiate(chickPrefab, chicksParent);

        ChickInventory newChickInventory = newChick.GetComponent<ChickInventory>();
        chicks.Add(newChickInventory);

        GameObject slot1 = Instantiate(slotPrefab, slotsParent);
        GameObject slot2 = Instantiate(slotPrefab, slotsParent);
        GameObject slot3 = Instantiate(slotPrefab, slotsParent);

        Transform[] newArray = new Transform[] { slot1.transform, slot2.transform, slot3.transform };

        slots.Add(slot1.transform);
        slots.Add(slot2.transform);
        slots.Add(slot3.transform);

        newChickInventory.uiSlots = newArray;
    }

    public void AddInventoryChick(Transform _pos)
    {
        // Add and setup new ChickInventory
        GameObject newChick = Instantiate(chickPrefab, chicksParent);
        newChick.transform.position = _pos.position;

        ChickInventory newChickInventory = newChick.GetComponent<ChickInventory>();
        chicks.Add(newChickInventory);

        GameObject slot1 = Instantiate(slotPrefab, slotsParent);
        GameObject slot2 = Instantiate(slotPrefab, slotsParent);
        GameObject slot3 = Instantiate(slotPrefab, slotsParent);

        Transform[] newArray = new Transform[] { slot1.transform, slot2.transform, slot3.transform };

        slots.Add(slot1.transform);
        slots.Add(slot2.transform);
        slots.Add(slot3.transform);

        newChickInventory.uiSlots = newArray;
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
        UpdateDecoChicks();
        return remainderItem;

    }

    public InventoryItem ReturnToInventory(InventoryItem _item)
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
                ReturnToInventory(remainderItem);
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
        UpdateDecoChicks();
        return remainderItem;
    }

    //public float ReturnToInventory(InventoryItem _item)
    //{
    //    // check for stack to combine with
    //    // if there is, combine stacks
    //    // if there is remainder, spawn new stack, call ReturnToInventory on new stack

    //    Transform openSlot = null;
    //    InventoryItem itemToCombine = GetItemToCombine(_item.itemID);
    //    float remainder = 0;

    //    if (itemToCombine != null)
    //    {
    //        remainder = CombineItems(_item, itemToCombine);
    //    }
    //    else
    //    {
    //        openSlot = GetOpenSlot();

    //        if (openSlot != null)
    //        {
    //            _item.transform.SetParent(openSlot, false);
    //            _item.transform.position = _item.transform.parent.position;
    //            _item.Lock(false);
    //        }
    //        else
    //        {
    //            print("INVENTORY FULL");
    //        }

    //    }
    //    UpdateDecoChicks();
    //    return remainder;
    //}

    public void UpdateDecoChicks()
    {
        for (int i = 0; i < chicks.Count; i++)
        {
            chicks[i].UpdateDecoSlots();
        }
    }
}
