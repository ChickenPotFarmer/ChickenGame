using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedBag : MonoBehaviour
{
    [Header("Info")]
    public string seedsName;

    private StrainProfile strain;
    private InventoryItem inventoryItem;

    private void Start()
    {
        if (!strain)
            strain = GetComponent<StrainProfile>();

        if (!inventoryItem)
            inventoryItem = GetComponent<InventoryItem>();

    }

    public void AddSeeds(int _amt)
    {
        inventoryItem.AddAmount(_amt);
    }

    public void RemoveSeeds(int _amt)
    {
        inventoryItem.AddAmount(-_amt);


        if (inventoryItem.amount <= 0)
            Destroy(gameObject);

    }
}
