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

        newChickInventory.uiSlots = newArray;
    }

    public void UpdateDecoChicks()
    {
        for (int i = 0; i < chicks.Count; i++)
        {
            chicks[i].UpdateDecoSlots();
        }
    }
}
