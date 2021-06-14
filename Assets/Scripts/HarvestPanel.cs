using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvestPanel : MonoBehaviour
{
    [Header("Weed Brick Settings")]
    public float maxPerBrick;
    public float maxPerBag = 10;

    [Header("Status")]
    public bool harvested;

    [Header("Setup")]
    public WeedPlant plant;
    public CanvasGroup cg;
    public GameObject weedBrickPrefab;
    public GameObject seedBagPrefab;
    public Transform slotsParent;
    public Transform[] slots;

    private List<InventoryItem> newBrickList;
    private InventoryController inventoryController;


    private void Start()
    {
        if (!inventoryController)
            inventoryController = InventoryController.instance.inventoryController.GetComponent<InventoryController>();

        slots = new Transform[slotsParent.childCount];

        //StartCoroutine(SlotsEmptyCheck());

        for (int i = 0; i < slotsParent.childCount; i++)
        {
            slots[i] = slotsParent.GetChild(i);
        }
        
    }

    public void HarvestSeeds(WeedPlant _plant, StrainProfile _strain)
    {
        float bagsNeeded = _plant.actualSeedYield / maxPerBag;
        bagsNeeded += 0.5f; // to make sure it rounds up
        bagsNeeded = Mathf.Round(bagsNeeded);

        GameObject newBag;

        InventoryItem newBagInventoryItem;

        for (int i = 0; i < bagsNeeded; i++)
        {
            if (_plant.actualSeedYield - ((bagsNeeded - 1) * maxPerBag) == 0 && i == bagsNeeded - 1)
            {
                //do nothing
            }
            else
            {
                newBag = Instantiate(seedBagPrefab, slots[i]);
                //newBrick.transform.position = new Vector2(0, 0);
                newBagInventoryItem = newBag.GetComponent<InventoryItem>();
                newBagInventoryItem.itemID = _strain.strainID;
                newBagInventoryItem.Lock(true);
                newBagInventoryItem.previousParent = slots[i];

                if (i != bagsNeeded - 1)
                {
                    newBagInventoryItem.SetAmount(maxPerBag);
                }
                else
                {
                    newBagInventoryItem.SetAmount(_plant.actualSeedYield - ((bagsNeeded - 1) * maxPerBag));
                }

                StrainProfile strainProf = newBag.GetComponent<StrainProfile>();
                strainProf.SetStrain(_strain);
                newBagInventoryItem.SetItemName();

                newBag.transform.position = newBag.transform.parent.position;
            }

        }
    }

    public void HarvestPlant(WeedPlant _plant, StrainProfile _strain)
    {
        SetPanelActive(true);

        if (!_plant.isPollinated)
        {
            float bricksNeeded = _plant.actualYield / maxPerBrick;
            bricksNeeded += 0.5f; // to make sure it rounds up
            bricksNeeded = Mathf.Round(bricksNeeded);

            GameObject newBrick;

            InventoryItem newBrickInventoryItem;

            for (int i = 0; i < bricksNeeded; i++)
            {
                newBrick = Instantiate(weedBrickPrefab, slots[i]);
                //newBrick.transform.position = new Vector2(0, 0);
                newBrickInventoryItem = newBrick.GetComponent<InventoryItem>();
                newBrickInventoryItem.itemID = _strain.strainID;
                newBrickInventoryItem.Lock(true);
                newBrickInventoryItem.previousParent = slots[i];

                if (i != bricksNeeded - 1)
                {
                    newBrickInventoryItem.SetAmount(maxPerBrick);
                }
                else
                {
                    newBrickInventoryItem.SetAmount(_plant.actualYield - ((bricksNeeded - 1) * maxPerBrick));
                }

                StrainProfile strainProf = newBrick.GetComponent<StrainProfile>();
                strainProf.SetStrain(_strain);

                newBrick.transform.position = newBrick.transform.parent.position;

            }
        }
        else
        {
            HarvestSeeds(_plant, _strain);
        }
    }

    public void ConfirmHarvest()
    {
        // clear plant and add to inventory

        SetPanelActive(false);
    }

    public void ClearInventory()
    {
        InventoryItem remainderItem;
        List<InventoryItem> slotItems = new List<InventoryItem>();

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].childCount != 0)
                slotItems.Add(slots[i].GetChild(0).GetComponent<InventoryItem>());
        }

        for (int i = 0; i < slotItems.Count; i++)
        {
            remainderItem = inventoryController.ReturnToInventory(slotItems[i]);

            if (remainderItem != null && remainderItem.amount > 0)
            {
                Debug.LogWarning("INVENTORY FULL");
            }

        }

        StartCoroutine(SlotsEmptyCheck());
    }

    IEnumerator SlotsEmptyCheck()
    {
        yield return new WaitForEndOfFrame();
        if (SlotsEmpty())
        {
            SetPanelActive(false);
            plant.ResetPlant();
        }    
    }

    public bool SlotsEmpty()
    {
        bool empty = true;

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].childCount != 0)
                empty = false;
        }

        return empty;
    }

    public void SetPanelActive(bool _active)
    {
        if (_active)
        {
            cg.alpha = 1;
            cg.interactable = true;
            cg.blocksRaycasts = true;
        }
        else
        {
            cg.alpha = 0;
            cg.interactable = false;
            cg.blocksRaycasts = false;
        }
    }
}
