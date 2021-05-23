using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [Header("Money")]
    public float moneyCarrying;

    [Header("Inventory Slots")]
    public Transform slotsParent;
    public List<Transform> slots;

    [Header("Settings")]
    public float gramsPerBrick;
    public GameObject slotPrefab;


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

        // Intialize Slots References
        if (slots.Count == 0)
        {
            for (int i = 0; i < slotsParent.childCount; i++)
            {
                slots.Add(slotsParent.GetChild(i));
            }
        }
    }

    public Transform[] AddInventoryChick()
    {
        GameObject slot1 = Instantiate(slotPrefab, slotsParent);
        GameObject slot2 = Instantiate(slotPrefab, slotsParent);
        GameObject slot3 = Instantiate(slotPrefab, slotsParent);

        Transform[] newArray = new Transform[] { slot1.transform, slot2.transform, slot3.transform };

        return newArray;
    }
}
