using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [Header("Money")]
    public float moneyCarrying;

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

        // Intialize Slots References
        if (slots.Count == 0)
        {
            for (int i = 0; i < slotsParent.childCount; i++)
            {
                slots.Add(slotsParent.GetChild(i));
            }
        }
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

    public Transform GetOpenSlot(string _id)
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

    public InventoryItem GetItemToCombine(string _id)
    {
        InventoryItem inventoryItem;
        InventoryItem itemToCombine = null;
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].childCount != 0)
            {
                inventoryItem = slots[i].GetChild(0).GetComponent<InventoryItem>();
                if (_id == inventoryItem.itemID && inventoryItem.amount < inventoryItem.maxAmount)
                {
                    openSlot = slots[i];
                    itemToCombine = inventoryItem;
                    break;
                }
            }
        }

        return itemToCombine;
    }

    public float CombineItems(InventoryItem _draggedItem, InventoryItem _childItem)
    {
        float remainder = 0;
        _childItem.AddAmount(_draggedItem.amount);
        _draggedItem.SetAmount(0);

        float diff = _childItem.amount - _childItem.maxAmount;

        //if there is remainder
        if (diff > 0)
        {
            _childItem.AddAmount(-diff);
            _childItem.Lock(false);
            _draggedItem.SetAmount(diff);
            _draggedItem.Lock(false);
            _draggedItem.ReturnToPreviousParent();
            remainder = diff;

        }
        else
        {
            Destroy(_draggedItem.gameObject);
            _childItem.Lock(false);

        }

        return remainder;

    }

    public float ReturnToInventory(InventoryItem _item)
    {
        Transform openSlot = GetOpenSlot();
        InventoryItem itemToCombine = null;
        float remainder = 0;

        if (openSlot != null)
        {
            _item.transform.SetParent(openSlot, false);
            _item.transform.position = _item.transform.parent.position;
            _item.Lock(false);
        }
        else
        {
            itemToCombine = GetItemToCombine(_item.itemID);

            if (itemToCombine != null)
            {
                remainder = CombineItems(_item, itemToCombine);
            }
            else
            {
                print("INVENTORY FULL");
            }

        }

        return remainder;
    }

    public void UpdateDecoChicks()
    {
        for (int i = 0; i < chicks.Count; i++)
        {
            chicks[i].UpdateDecoSlots();
        }
    }
}
