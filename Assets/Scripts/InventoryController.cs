using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [Header("Status")]
    public float dryGramsCarrying;
    public float wetGramsCarrying;
    public float totalGramsCarrying;
    public float moneyCarrying;
    public bool amountOkToAdd;
    public float maxInventory;
    public int bricksCarrying;

    [Header("Settings")]
    public float gramsPerBrick;

    [Header("Chicks")]
    public ChickInventory[] chicks;
    public int currentChick;
    public Transform chicksParent;

    private float lastInventoryUpdate;

    public static InventoryController instance;
    [HideInInspector]
    public GameObject inventoryController;

    private void Awake()
    {
        instance = this;
        inventoryController = gameObject;
    }

    private void Start()
    {
        UpdateChickRefs();
        maxInventory = chicks.Length * 3 * gramsPerBrick;
    }

    private void Update()
    {
        totalGramsCarrying = wetGramsCarrying + dryGramsCarrying;

        if (totalGramsCarrying != lastInventoryUpdate)
            UpdateChicksInventory();
    }

    public bool AddWetGrams(float _amt)
    {
        amountOkToAdd = true;
        maxInventory = chicks.Length * 3 * gramsPerBrick;

        if (wetGramsCarrying + dryGramsCarrying + _amt <= maxInventory)
        {
            wetGramsCarrying += _amt;
            UpdateChicksInventory();

            // add sprites to inventory
        }
        else
        {
            amountOkToAdd = false;
        }

        return amountOkToAdd;
    }

    public void DropWetGrams()
    {
        wetGramsCarrying = 0;
        UpdateChicksInventory();
    }

    public bool AddDryGrams(float _amt)
    {
        amountOkToAdd = true;
        maxInventory = chicks.Length * 3 * gramsPerBrick;

        if (wetGramsCarrying + dryGramsCarrying + _amt <= maxInventory)
        {
            dryGramsCarrying += _amt;
            UpdateChicksInventory();
        }
        else
        {
            amountOkToAdd = false;
        }

        return amountOkToAdd;
    }

    public bool AddCash(float _amt)
    {
        bool amountOk = true;
        moneyCarrying += _amt;
        return amountOk;
    }

    public void AddWeedToInventory()
    {

    }

    public void UpdateChicksInventory()
    {
        UpdateChickRefs();
        currentChick = 0;
        bricksCarrying = 0;
        for (int i = 0; i < chicks.Length; i++)
        {
            chicks[i].RemoveAllBricks();
        }

        totalGramsCarrying = wetGramsCarrying + dryGramsCarrying;
        lastInventoryUpdate = totalGramsCarrying;

        float bricks = totalGramsCarrying / gramsPerBrick;
        bricksCarrying = Mathf.RoundToInt(bricks);

        for (int i = 0; i < bricksCarrying; i++)
        {
            if (!chicks[currentChick].slotsFull)
            {
                chicks[currentChick].AddBrick();
            }
            else
            {
                currentChick++;
                if (currentChick != chicks.Length)
                {
                    chicks[currentChick].AddBrick();
                }
                else
                {
                    print("INVENTORY FULL!");
                }

            }
        }
    }

    public void UpdateChickRefs()
    {
        chicks = new ChickInventory[chicksParent.childCount];
        for (int i = 0; i < chicksParent.childCount; i++)
        {
            chicks[i] = chicksParent.GetChild(i).GetComponent<ChickInventory>();
        }
    }

    

}
