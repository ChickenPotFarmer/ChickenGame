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
    public float policeBaseFine; 
    public float policeFinePerGram; 
    public Transform dragParent;
    public Canvas inventoryCanvas;
    public float gramsPerBrick;
    public GameObject slotPrefab;
    public CanvasGroup inventoryCg;


    public static InventoryController instance;
    [HideInInspector]
    public GameObject inventoryController;

    private Transform openSlot;
    private ThirdPersonController thirdPersonController;
    private Bank bank;

    private void Awake()
    {
        instance = this;
        inventoryController = gameObject;
    }

    private void Start()
    {
        if (!thirdPersonController)
            thirdPersonController = ThirdPersonController.instance.thirdPerson.GetComponent<ThirdPersonController>();
        // Intialize Slots References
        slots = CreateSlots(slotsParent);
    }

    private List<Transform> CreateSlots(Transform _slotsParent)
    {
        List<Transform> newSlotsList = new List<Transform>();

        for (int i = 0; i < _slotsParent.childCount; i++)
        {
            newSlotsList.Add(_slotsParent.GetChild(i));
        }

        return newSlotsList;
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
            thirdPersonController.UnlockCursor();
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
    // player will need to deposit money at bank or atm to buy stuff online

    public bool CheckIfCanAfford(float _amt)
    {
        if (GetCashOnHand() >= _amt)
            return true;
        else
            return false;

    }

    #region InventoryChicks

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

    #endregion
    #region Get Open Slots
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

    public Transform GetOpenSlot(Transform _slotsParent)
    {
        Transform[] tempSlots = new Transform[_slotsParent.childCount];

        for (int i = 0; i < _slotsParent.childCount; i++)
        {
            tempSlots[i] = _slotsParent.GetChild(i);
        }
        openSlot = null;

        for (int i = 0; i < tempSlots.Length; i++)
        {
            if (tempSlots[i].childCount == 0)
            {
                openSlot = tempSlots[i];
                break;
            }
        }

        return openSlot;
    }

    public Transform GetOpenSlot(List<Transform> _slotsList)
    {
        openSlot = null;

        for (int i = 0; i < _slotsList.Count; i++)
        {
            if (_slotsList[i].childCount == 0)
            {
                openSlot = _slotsList[i];
                break;
            }
        }

        return openSlot;
    }

    public ItemSlot GetOpenSlot(List<ItemSlot> _slotsList)
    {
        ItemSlot itemSlot = null;

        for (int i = 0; i < _slotsList.Count; i++)
        {
            if (!_slotsList[i].HasItem())
            {
                itemSlot = _slotsList[i];
                break;
            }
        }

        return itemSlot;
    }

    #endregion
    #region Combine Items
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

    public InventoryItem GetItemToCombine(InventoryItem _itemToCombine, List<Transform> _slotsList)
    {
        InventoryItem inventoryItem;
        InventoryItem itemToCombine = null;
        for (int i = 0; i < _slotsList.Count; i++)
        {
            if (_slotsList[i].childCount != 0)
            {
                inventoryItem = _slotsList[i].GetChild(0).GetComponent<InventoryItem>();
                if (_itemToCombine.itemID == inventoryItem.itemID && inventoryItem.amount < inventoryItem.maxAmount && _itemToCombine.CompareTag(inventoryItem.tag))
                {
                    openSlot = _slotsList[i];
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
    #endregion
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
                if (_item.previousParent == null)
                    _item.previousParent = openSlot;
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

    public InventoryItem ReturnToInventory(InventoryItem _item, Transform _slotsParent)
    {
        // check for stack to combine with
        // if there is, combine stacks
        // if there is remainder, spawn new stack, call ReturnToInventory on new stack

        List<Transform> tempSlots = new List<Transform>();

        tempSlots = CreateSlots(_slotsParent);

        Transform openSlot = null;
        InventoryItem itemToCombine = GetItemToCombine(_item, tempSlots);
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
            openSlot = GetOpenSlot(tempSlots);

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

            List<InventoryItem> cashItems = new List<InventoryItem>();
            InventoryItem foundCash;


            for (int i = 0; i < slots.Count; i++)
            {
                if (slots[i].childCount != 0)
                {
                    foundCash = slots[i].GetChild(0).GetComponent<InventoryItem>();

                    if (foundCash.itemID == "CASH")
                    {
                        cashItems.Add(foundCash);
                    }
                }
            }

            float amtToRemove = _amt;

            // Sory items if more than one
            if (cashItems.Count > 1)
            {
                InventoryItem tempItem;

                for (int i = 0; i < cashItems.Count - 1; i++)
                {
                    if (cashItems[i].amount > cashItems[i + 1].amount)
                    {
                        tempItem = cashItems[i];
                        cashItems[i] = cashItems[i + 1];
                        cashItems[i + 1] = tempItem;
                        i = -1;
                    }
                }
            }


            for (int i = 0; i < cashItems.Count; i++)
            {

                if (cashItems[i].amount >= amtToRemove)
                {
                    cashItems[i].AddAmount(-amtToRemove);
                    amtToRemove = 0;
                    break;
                }
                else
                {
                    amtToRemove -= cashItems[i].amount;
                    cashItems[i].SetAmount(0);
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

    public void InventoryToInventoryTransfer(Transform _fromInventoryParent, Transform _toInventoryParent, InventoryItem _itemClicked)
    {
        InventoryItem remainderItem;
        Transform[] tempSlots = new Transform[_fromInventoryParent.childCount];

        for (int i = 0; i < _fromInventoryParent.childCount; i++)
        {
            tempSlots[i] = _fromInventoryParent.GetChild(i);
        }

        List<InventoryItem> slotItems = new List<InventoryItem>();
        InventoryItem tempItem;

        for (int i = 0; i < tempSlots.Length; i++)
        {
            if (tempSlots[i].childCount != 0)
            {
                tempItem = tempSlots[i].GetChild(0).GetComponent<InventoryItem>();
                if (tempItem.itemID == _itemClicked.itemID)
                {
                    if (_itemClicked.isBrick)
                    {
                        if (tempItem.isBrick)
                            slotItems.Add(tempSlots[i].GetChild(0).GetComponent<InventoryItem>());

                    }
                    else
                    {
                        if (!tempItem.isBrick)
                            slotItems.Add(tempSlots[i].GetChild(0).GetComponent<InventoryItem>());
                    }
                }
            }
        }

        for (int i = 0; i < slotItems.Count; i++)
        {
            remainderItem = ReturnToInventory(slotItems[i], _toInventoryParent);

            if (remainderItem != null && remainderItem.amount > 0)
            {
                Debug.LogWarning("INVENTORY FULL");
            }

        }

    }

    public void InventoryToInventoryTransfer(Transform _fromInventoryParent, Transform _toInventoryParent)
    {
        InventoryItem remainderItem;
        Transform[] tempSlots = new Transform[_fromInventoryParent.childCount];

        for (int i = 0; i < _fromInventoryParent.childCount; i++)
        {
            tempSlots[i] = _fromInventoryParent.GetChild(i);
        }

        List<InventoryItem> slotItems = new List<InventoryItem>();

        for (int i = 0; i < tempSlots.Length; i++)
        {
            if (tempSlots[i].childCount != 0)
            {
                if (tempSlots[i].GetChild(0).GetComponent<InventoryItem>())
                    slotItems.Add(tempSlots[i].GetChild(0).GetComponent<InventoryItem>());
            }
        }

        for (int i = 0; i < slotItems.Count; i++)
        {
            remainderItem = ReturnToInventory(slotItems[i], _toInventoryParent);

            if (remainderItem != null && remainderItem.amount > 0)
            {
                Debug.LogWarning("INVENTORY FULL");
            }

        }

    }

    public void InventoryToBuyerTransfer(Buyer _buyer)
    {
        // Create list of Weed Bricks in inventory
        List<InventoryItem> bricksInInventory = new List<InventoryItem>();
        InventoryItem foundItem;

        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].childCount != 0)
            {
                foundItem = slots[i].GetChild(0).GetComponent<InventoryItem>();
                if (foundItem)
                {
                    if (foundItem.CompareTag("UI Weed Brick"))
                    {
                        bricksInInventory.Add(foundItem);
                    }
                }
            }
        }

        // create array of ItemSlots from buyer
        List<ItemSlot> itemSlots = new List<ItemSlot>();

        for (int i = 0; i < _buyer.slots.Length; i++)
        {
            itemSlots.Add(_buyer.slots[i].GetComponent<ItemSlot>());
        }

        ItemSlot openSlot;
        StrainProfile strain;
        // check weed bricks
        for (int i = 0; i < bricksInInventory.Count; i++)
        {
            strain = bricksInInventory[i].gameObject.GetComponent<StrainProfile>();

            if (strain != null)
                print("strain found");
            if (_buyer.PassesInspection(strain))
            {
                openSlot = null;
                openSlot = GetOpenSlot(itemSlots);

                if (openSlot != null)
                {
                    openSlot.ManualDrop(bricksInInventory[i].gameObject);
                }
                else
                {
                    // spawn in more slots (fuck it who cares)
                }
            }
        }
    }

    public void InventoryToBuyerTransfer(Buyer _buyer, string _id)
    {
        // Create list of Weed Bricks in inventory
        List<InventoryItem> bricksInInventory = new List<InventoryItem>();
        InventoryItem foundItem;

        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].childCount != 0)
            {
                foundItem = slots[i].GetChild(0).GetComponent<InventoryItem>();
                if (foundItem)
                {
                    if (foundItem.CompareTag("UI Weed Brick") && foundItem.itemID == _id)
                    {
                        bricksInInventory.Add(foundItem);
                    }
                }
            }
        }

        // create array of ItemSlots from buyer
        List<ItemSlot> itemSlots = new List<ItemSlot>();

        for (int i = 0; i < _buyer.slots.Length; i++)
        {
            itemSlots.Add(_buyer.slots[i].GetComponent<ItemSlot>());
        }

        ItemSlot openSlot;
        StrainProfile strain;
        // check weed bricks
        for (int i = 0; i < bricksInInventory.Count; i++)
        {
            strain = bricksInInventory[i].gameObject.GetComponent<StrainProfile>();

            if (strain != null)
                print("strain found");
            if (_buyer.PassesInspection(strain))
            {
                openSlot = null;
                openSlot = GetOpenSlot(itemSlots);

                if (openSlot != null)
                {
                    openSlot.ManualDrop(bricksInInventory[i].gameObject);
                }
                else
                {
                    // spawn in more slots (fuck it who cares)
                }
            }
        }
    }

    public void PoliceWipeInventory()
    {
        List<InventoryItem> slotItems = new List<InventoryItem>();
        InventoryItem foundItem;
        float gramsFound = 0;
        float fineTotal = 0;
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].childCount != 0)
            {
                foundItem = slots[i].GetChild(0).GetComponent<InventoryItem>();
                if (foundItem)
                {
                    slotItems.Add(foundItem);
                }
            }
        }

        for (int i = 0; i < slotItems.Count; i++)
        {
            if (slotItems[i].isStrain)
            {
                gramsFound += slotItems[i].amount;

                Destroy(slotItems[i].gameObject);
            }
        }

        fineTotal = Mathf.Round((policeBaseFine * Xp.GetPlayerLevel()) + (gramsFound * policeFinePerGram));

        // If player cant pay fine out of inventory or bank account, add it to loan
        if (!RemoveCash(fineTotal))
        {
            if (!bank)
                bank = Bank.instance.bank.GetComponent<Bank>();

            if (bank.bankAccount >= fineTotal)
            {
                bank.WireTransfer(-fineTotal);
                Alerts.DisplayMessage(gramsFound.ToString("n0") + " grams found during search. Fined $" + fineTotal.ToString("n0") + ". Fine has been removed directly from bank account.");

            }
            else
            {
                bank.AddToLoan(fineTotal);
                Alerts.DisplayMessage(gramsFound.ToString("n0") + " grams found during search. Fined $" + fineTotal.ToString("n0") + ". Fine has been added to bank loan.");
            }
        }
        else
        {
            Alerts.DisplayMessage(gramsFound.ToString("n0") + " grams found during search. Fined $" + fineTotal.ToString("n0") + ". Fine has been paid in cash from inventory.");

        }

    }
}
