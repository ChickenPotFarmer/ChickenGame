using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvestPanel : MonoBehaviour
{
    [Header("Weed Brick Settings")]
    public float maxPerBrick;
    public float maxPerBag = 10;
    public float maxPerTrimmings = 200;

    [Header("Status")]
    public bool harvested;

    [Header("Setup")]
    public WeedPlant plant;
    public CanvasGroup cg;
    public GameObject weedBrickPrefab;
    public GameObject seedBagPrefab;
    public GameObject trimmingsPrefab;
    public Transform slotsParent;
    public Transform[] slots;

    private List<InventoryItem> newBrickList;
    private InventoryController inventoryController;
    private ThirdPersonController thirdPersonController;
    private BreederMaster breederMaster;
    private Trimmer trimmer;


    private void Start()
    {
        if (!inventoryController)
            inventoryController = InventoryController.instance.inventoryController.GetComponent<InventoryController>();

        if (!breederMaster)
            breederMaster = BreederMaster.instance.breederMaster.GetComponent<BreederMaster>();

        if (!trimmer)
            trimmer = Trimmer.instance.trimmer.GetComponent<Trimmer>();

        if (!thirdPersonController)
            thirdPersonController = ThirdPersonController.instance.thirdPerson.GetComponent<ThirdPersonController>();

        slots = new Transform[slotsParent.childCount];

        //StartCoroutine(SlotsEmptyCheck());

        for (int i = 0; i < slotsParent.childCount; i++)
        {
            slots[i] = slotsParent.GetChild(i);
        }
        
    }

    public void HarvestTrimmings(WeedPlant _plant)
    {
        float trimmingsNeeded = _plant.actualTrimmingsYield / maxPerTrimmings;
        trimmingsNeeded += 0.5f; // to make sure it rounds up
        trimmingsNeeded = Mathf.Round(trimmingsNeeded);

        GameObject newTrimmings;

        InventoryItem newTrimmingsInventoryItem;

        for (int i = 0; i < trimmingsNeeded; i++)
        {
            if (_plant.actualTrimmingsYield - ((trimmingsNeeded - 1) * maxPerTrimmings) == 0 && i == trimmingsNeeded - 1)
            {
                //do nothing
            }
            else
            {
                newTrimmings = Instantiate(trimmingsPrefab, slots[i]);
                //newBrick.transform.position = new Vector2(0, 0);
                newTrimmingsInventoryItem = newTrimmings.GetComponent<InventoryItem>();

                newTrimmingsInventoryItem.Lock(true);
                newTrimmingsInventoryItem.previousParent = slots[i];

                if (i != trimmingsNeeded - 1)
                {
                    newTrimmingsInventoryItem.SetAmount(maxPerTrimmings);
                }
                else
                {
                    newTrimmingsInventoryItem.SetAmount(_plant.actualTrimmingsYield - ((trimmingsNeeded - 1) * maxPerTrimmings));
                }



                newTrimmings.transform.position = newTrimmings.transform.parent.position;
            }

        }
    }

    public void HarvestSeeds(WeedPlant _plant, GameObject _bredSeedBag)
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
                newBag = Instantiate(_bredSeedBag, slots[i]); 
                //newBrick.transform.position = new Vector2(0, 0);
                newBagInventoryItem = newBag.GetComponent<InventoryItem>();
                newBagInventoryItem.Lock(true);
                newBagInventoryItem.isStrain = true;
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
                //strainProf.SetStrain(_strain);
                strainProf.itemComp = newBagInventoryItem;
                newBagInventoryItem.itemName = strainProf.strainName;
                newBagInventoryItem.SetItemName();

                newBag.transform.position = newBag.transform.parent.position;
            }

        }

        Destroy(_bredSeedBag);
    }

    public void HarvestPlant(WeedPlant _plant, StrainProfile _strain, bool _openInventory)
    {
        if (_openInventory)
            SetPanelActive(true);

        if (!_plant.isPollinated && !_plant.isMale)
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
                newBrickInventoryItem.isStrain = true;
                newBrickInventoryItem.isBrick = true;
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
                newBrickInventoryItem.itemName = strainProf.strainName;
                newBrickInventoryItem.SetItemName();
                newBrick.transform.position = newBrick.transform.parent.position;

            }
            _plant.harvested = true;

        }
        else if (_plant.isPollinated && !_plant.isMale)
        {
            GameObject newBredSeedBag = breederMaster.Breed(_plant, _plant.fatherPlantStrain); 
            if (_plant.currentStrain != _plant.fatherPlantStrain)
            {
                print("New Strain Created");
            }
            else
            {
                newBredSeedBag.GetComponent<StrainProfile>().SetStrain(_strain);
            }
            

            HarvestSeeds(_plant, newBredSeedBag);
            _plant.harvested = true;

        }
        else if (_plant.isMale)
        {
            HarvestTrimmings(_plant);
            _plant.harvested = true;

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

    private bool trimmerWasOn;

    public void SetPanelActive(bool _active)
    {
        if (_active)
        {
            cg.alpha = 1;
            cg.interactable = true;
            cg.blocksRaycasts = true;
            trimmerWasOn = trimmer.trimmerOn;
            thirdPersonController.UnlockCursor();
            trimmer.trimmerOn = false;
        }
        else
        {
            cg.alpha = 0;
            cg.interactable = false;
            cg.blocksRaycasts = false;
            if (trimmerWasOn)
            {
                trimmer.trimmerOn = true;
                thirdPersonController.LockCursor();
            }
        }
    }
}
