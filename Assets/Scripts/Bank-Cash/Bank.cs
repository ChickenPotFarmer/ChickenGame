using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bank : MonoBehaviour
{
    [Header("Status")]
    public float maxCashPerStack;

    [Header("Setup")]
    public GameObject cashPrefab;

    private InventoryController inventoryController;

    public static Bank instance;
    [HideInInspector]
    public GameObject bank;

    private void Awake()
    {
        instance = this;
        bank = gameObject;
    }

    private void Start()
    {
        if (!inventoryController)
            inventoryController = InventoryController.instance.inventoryController.GetComponent<InventoryController>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(","))
        {
            PayAmount(100);
            Xp.AddXp(50);
        }
        else if (Input.GetKeyDown("."))
            RequestCash(100);
    }

    public bool PayAmount(float _amt)
    {
        bool hasEnoughCash = false;

        if (inventoryController.RemoveCash(_amt))
        {
            hasEnoughCash = true;
            Alerts.DisplayMessage("$" + _amt.ToString("n0") + " removed from Inventory.");

        }

        return hasEnoughCash;
    }

    // obsolete because money is now an object.
    public bool RequestCash(float _amt)
    {
        bool roomInInventory;

        if (inventoryController.CanTakeItem("CASH", _amt))
        {
            roomInInventory = true;

            float bricksNeeded = _amt / maxCashPerStack;
            bricksNeeded += 0.5f; // to make sure it rounds up
            bricksNeeded = Mathf.Round(bricksNeeded);

            GameObject newBrick;

            InventoryItem newBrickInventoryItem;

            for (int i = 0; i < bricksNeeded; i++)
            {
                newBrick = Instantiate(cashPrefab);
                //newBrick.transform.position = new Vector2(0, 0);
                newBrickInventoryItem = newBrick.GetComponent<InventoryItem>();


                if (i != bricksNeeded - 1)
                {
                    newBrickInventoryItem.SetAmount(maxCashPerStack);
                }
                else
                {
                    newBrickInventoryItem.SetAmount(_amt - ((bricksNeeded - 1) * maxCashPerStack));
                }

                inventoryController.ReturnToInventory(newBrickInventoryItem);

            }

            //GameObject newCash = Instantiate(cashPrefab);
            //InventoryItem newItem = newCash.GetComponent<InventoryItem>();

            //newItem.SetAmount(_amt);
            //inventoryController.ReturnToInventory(newItem);


        }
        else
        {
            roomInInventory = false;
        }

        return roomInInventory;
    }

    // assumes inventory has room
    public void RequestCash(float _amt, Transform _targetInventory, bool _lockAfterSpawn)
    {
        Transform[] targetSlots = new Transform[_targetInventory.childCount];
        float bricksNeeded = _amt / maxCashPerStack;
        bricksNeeded += 0.5f; // to make sure it rounds up
        bricksNeeded = Mathf.Round(bricksNeeded);

        GameObject newBrick;

        InventoryItem newBrickInventoryItem;

        // intialize slots
        for (int i = 0; i < _targetInventory.childCount; i++)
        {
            targetSlots[i] = _targetInventory.GetChild(i);
        }

        // Spawn and set prfabs
        for (int i = 0; i < bricksNeeded; i++)
        {
            newBrick = Instantiate(cashPrefab);
            newBrickInventoryItem = newBrick.GetComponent<InventoryItem>();


            if (i != bricksNeeded - 1)
            {
                newBrickInventoryItem.SetAmount(maxCashPerStack);
            }
            else
            {
                newBrickInventoryItem.SetAmount(_amt - ((bricksNeeded - 1) * maxCashPerStack));
            }

            newBrickInventoryItem.SetNewParent(targetSlots[i]);

            if (_lockAfterSpawn)
                newBrickInventoryItem.Lock(true);

        }
    }
}
