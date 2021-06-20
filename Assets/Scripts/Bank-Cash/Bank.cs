using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bank : MonoBehaviour
{
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


    public bool RequestCash(float _amt)
    {
        bool roomInInventory;

        if (inventoryController.CanTakeItem("CASH", _amt))
        {
            roomInInventory = true;

            float bricksNeeded = _amt / 1000;
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
                    newBrickInventoryItem.SetAmount(1000);
                }
                else
                {
                    newBrickInventoryItem.SetAmount(_amt - ((bricksNeeded - 1) * 1000));
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
}
