using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryController : MonoBehaviour
{
    [Header("Money")]
    public float moneyCarrying;
    public Text moneyTxt;

    [Header("Status")]
    public bool inventoryActive;

    [Header("Inventory Chicks")]
    public GameObject chickPrefab;
    public Transform chicksParent;
    public List<ChickInventory> chicks;

    [Header("Inventory Slots")]
    public Transform slotsParent;
    public List<Transform> slots;

    [Header("Settings")]
    public Transform dragParent;
    public Canvas inventoryCanvas;
    public float gramsPerBrick;
    public GameObject slotPrefab;
    public CanvasGroup inventoryCg;


    public static InventoryController instance;
    [HideInInspector]
    public GameObject inventoryController;

    private Transform openSlot;

    private void Awake()
    {
        instance = this;
        inventoryController = gameObject;
    }

    private void Start()
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

    public void ToggleInventoryPanel()
    {
        if (inventoryActive)
        {
            SetInventoryActive(false);
        }
        else
        {
            SetInventoryActive(true);
        }
    }

    private void SetInventoryActive(bool _active)
    {
        if (_active)
        {
            inventoryCg.alpha = 1;
            inventoryCg.interactable = true;
            inventoryCg.blocksRaycasts = true;
            inventoryActive = true;
        }
        else
        {
            inventoryCg.alpha = 0;
            inventoryCg.interactable = false;
            inventoryCg.blocksRaycasts = false;
            inventoryActive = false;
        }
    }

    // TO-DO:
    // Change this so that money is an object in the game
    // player will need to deposit money at bank or atm to buy stuff online

    public bool CheckIfCanAfford(float _amt)
    {
        float newAmt = moneyCarrying - _amt;
        if (newAmt >= 0)
            return true;
        else
            return false;

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

        if (_item)
            _item.UpdateCurrentParent();

        if (remainderItem)
            remainderItem.UpdateCurrentParent();

        UpdateDecoChicks();
        
        return remainderItem;
    }

    public bool CanTakeItem(string _itemID, float _amt)
    {
        bool hasRoom = false;
        InventoryItem inventoryItem;

        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].childCount != 0)
            {
                inventoryItem = slots[i].GetChild(0).GetComponent<InventoryItem>();
                if (_itemID == inventoryItem.itemID && (inventoryItem.amount + _amt) < inventoryItem.maxAmount)
                {
                    hasRoom = true;
                    break;
                }
            }
            else
            {
                hasRoom = true;
            }
        }

        //Handle sound effects here too
        if (!hasRoom)
            print("No Room In Inventory");

        return hasRoom;
    }

    public float GetCashOnHand()
    {
        float cashOnHand = 0;
        InventoryItem foundCash;

        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].childCount != 0)
            {
                foundCash = slots[i].GetChild(0).GetComponent<InventoryItem>();

                if (foundCash.itemID == "CASH")
                {
                    cashOnHand += foundCash.amount;
                }
            }
        }

        return cashOnHand;
    }

    public bool RemoveCash(float _amt)
    {
        bool hasEnoughMoney = false;

        if (GetCashOnHand() >= _amt)
        {
            hasEnoughMoney = true;
            float amtToRemove = _amt;

            InventoryItem foundCash;

            for (int i = 0; i < slots.Count; i++)
            {
                if (slots[i].childCount != 0)
                {
                    foundCash = slots[i].GetChild(0).GetComponent<InventoryItem>();

                    if (foundCash.itemID == "CASH")
                    {
                        if (foundCash.amount >= amtToRemove)
                        {
                            foundCash.AddAmount(-amtToRemove);
                            amtToRemove = 0;
                            break;
                        }
                        else
                        {
                            amtToRemove -= foundCash.amount;
                            foundCash.SetAmount(0);
                        }
                    }
                }
            }
        }

        return hasEnoughMoney;
    }

    public void UpdateDecoChicks()
    {
        for (int i = 0; i < chicks.Count; i++)
        {
            chicks[i].UpdateDecoSlots();
        }
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
            remainderItem = ReturnToInventory(slotItems[i]);

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
                if (tempSlots[i].GetChild(0).GetComponent<InventoryItem>().itemID == _itemId)
                    slotItems.Add(tempSlots[i].GetChild(0).GetComponent<InventoryItem>());
            }
        }

        for (int i = 0; i < slotItems.Count; i++)
        {
            remainderItem = ReturnToInventory(slotItems[i]);

            if (remainderItem != null && remainderItem.amount > 0)
            {
                Debug.LogWarning("INVENTORY FULL");
            }

        }
    }
}
