using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickInventory : MonoBehaviour
{
    [Header("UI Slots")]
    public Transform[] uiSlots;

    [Header("Brick Slots")]
    public Transform[] decoSlots;
    public Transform brickParent;
    public bool slotsFull;

    [Header("Deco Prefabs")]
    public GameObject brickPrefab;
    public GameObject seedPrefab;
    public GameObject trimmingsPrefab;

    private int nextOpenSlot;
    private InventoryController inventoryController;

    private void Start()
    {
        if (!inventoryController)
            inventoryController = InventoryController.instance.inventoryController.GetComponent<InventoryController>();


        decoSlots = new Transform[brickParent.childCount];
        for (int i = 0; i < brickParent.childCount; i++)
        {
            decoSlots[i] = brickParent.GetChild(i);
        }

        UpdateDecoSlots();
    }

    public void UpdateDecoSlots()
    {
        for (int i = 0; i < decoSlots.Length; i++)
        {
            if (decoSlots[i].childCount != 0)
                Destroy(decoSlots[i].GetChild(0).gameObject);
        }

        for (int i = 0; i < uiSlots.Length; i++)
        {
            if (uiSlots[i].childCount != 0)
            {
                string childTag = uiSlots[i].GetChild(0).tag;

                switch (childTag)
                {
                    case "UI Seed Bag":
                        Instantiate(seedPrefab, decoSlots[i]);
                        break;

                    case "UI Weed Brick":
                        Instantiate(brickPrefab, decoSlots[i]);
                        break;

                    case "UI Trimmings":
                        Instantiate(trimmingsPrefab, decoSlots[i]);
                        break;
                }
            }
        }
    }
}
