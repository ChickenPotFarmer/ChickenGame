using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvestPanel : MonoBehaviour
{
    [Header("Weed Brick Settings")]
    public float maxPerBrick;

    [Header("Setup")]
    public CanvasGroup cg;
    public GameObject weedBrickPrefab;
    public Transform slotsParent;
    public Transform[] slots;

    private List<InventoryItem> newBrickList;

    public static HarvestPanel instance;
    [HideInInspector]
    public GameObject harvestPanel;

    private void Awake()
    {
        instance = this;
        harvestPanel = gameObject;
    }

    private void Start()
    {
        slots = new Transform[slotsParent.childCount];


        for (int i = 0; i < slotsParent.childCount; i++)
        {
            slots[i] = slotsParent.GetChild(i);
        }
        
    }

    public void HarvestPlant(WeedPlant _plant, StrainProfile _strain)
    {
        SetPanelActive(true);
        float bricksNeeded = _plant.actualYield / maxPerBrick;
        bricksNeeded += 0.5f; // to make sure it rounds up
        bricksNeeded = Mathf.Round(bricksNeeded);

        GameObject newBrick;

        for (int i = 0; i < bricksNeeded; i++)
        {
            newBrick = Instantiate(weedBrickPrefab, slots[i]);
            //newBrick.transform.position = new Vector2(0, 0);

            if (i != bricksNeeded - 1)
            {
                newBrick.GetComponent<InventoryItem>().SetAmount(maxPerBrick);
            }
            else
            {
                newBrick.GetComponent<InventoryItem>().SetAmount(_plant.actualYield - ((bricksNeeded - 1) * maxPerBrick));
            }

            StrainProfile strainProf = newBrick.GetComponent<StrainProfile>();
            strainProf.SetStrain(_strain);

            newBrick.transform.position = newBrick.transform.parent.position;

        }
    }

    public void ConfirmHarvest()
    {
        // clear plant and add to inventory

        SetPanelActive(false);
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
