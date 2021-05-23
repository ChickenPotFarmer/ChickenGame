using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedBag : MonoBehaviour
{
    [Header("Info")]
    public string seedsName;
    public int seeds = 10;

    private StrainProfile strain;
    private InventoryItem inventoryItem;

    private void Start()
    {
        if (!strain)
            strain = GetComponent<StrainProfile>();

        if (!inventoryItem)
            inventoryItem = GetComponent<InventoryItem>();

        inventoryItem.uiName.text = strain.strainName;
        inventoryItem.uiAmount.text = seeds.ToString();

    }

    public void AddSeeds(int _amt)
    {
        seeds += _amt;

        inventoryItem.uiAmount.text = seeds.ToString();
    }

    public void RemoveSeeds(int _amt)
    {
        seeds -= _amt;

        if (seeds <= 0)
            Destroy(gameObject);

        inventoryItem.uiAmount.text = seeds.ToString();
    }
}
