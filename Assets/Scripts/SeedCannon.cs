using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SeedCannon : MonoBehaviour
{
    [Header("Strain")]
    public StrainProfile currentStrain;
    public InventoryItem seedBagItem;

    [Header("Status")]
    public bool cannonOn;

    [Header("Setup")]
    public CanvasGroup cg;
    public ItemSlot seedSlot;
    public Transform debugTarget;
    public Transform firePoint;
    public GameObject seedProjectile;
    public GameObject cannonModel;

    [Header("UI")]
    public Text seedNameTxt;

    public static SeedCannon instance;
    [HideInInspector]
    public GameObject seedCannon;

    private StrainInfoUI strainInfoPanel;

    private void Awake()
    {
        instance = this;
        seedCannon = gameObject;
    }

    private void Start()
    {
        if (cannonOn)
            cannonModel.SetActive(true);
        else
            cannonModel.SetActive(false);

        if (!strainInfoPanel)
            strainInfoPanel = StrainInfoUI.instance.strainInfoUI.GetComponent<StrainInfoUI>();
    }


    public void FireCannon(Transform _target)
    {
        if (seedSlot.HasItem())
        {
            GameObject newSeed = Instantiate(seedProjectile);
            newSeed.transform.position = firePoint.position;
            Seed seedComp = newSeed.GetComponent<Seed>();

            seedComp.target = _target;

            WeedPlant weedPlant = _target.GetComponent<WeedPlant>();

            if (weedPlant)
                weedPlant.hasSeed = true;

            if (currentStrain != null)
                newSeed.GetComponent<Seed>().currentStrain.SetStrain(currentStrain);
            else
                Debug.LogWarning("Error in SeedCannon, currentStrain is null.");

            if (seedBagItem.AddAmount(-1))
            {
                ResetCannon();
            }
        }
    }

    public void ToggleCannon()
    {
        if (cannonOn)
        {
            cannonOn = false;
            cannonModel.SetActive(false);
            SetPanelActive(false);
        }
        else
        {
            cannonOn = true;
            cannonModel.SetActive(true);
            SetPanelActive(true);
        }
    }

    public void ToggleCannon(bool _on)
    {
        if (!_on)
        {
            cannonOn = false;
            cannonModel.SetActive(false);
            SetPanelActive(false);
        }
        else
        {
            cannonOn = true;
            cannonModel.SetActive(true);
            SetPanelActive(true);
        }
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

    public void OnItemDrop()
    {
        GameObject seedBag = seedSlot.GetItem();
        currentStrain.SetStrain(seedBag.GetComponent<StrainProfile>());
        seedBagItem = seedBag.GetComponent<InventoryItem>();

        seedBagItem.Lock(true);


        // update title text
        seedNameTxt.text = currentStrain.strainName;

        //  update seed cannon
        seedSlot.acceptsNone = true;

        // update strain info?
    }

    public void ShowStrainInfo()
    {
        if (seedSlot.HasItem())
            strainInfoPanel.SetStrainInfoActive(currentStrain);
    }

    public void ResetCannon()
    {
        seedNameTxt.text = "No Seeds Loaded";
        seedSlot.acceptsNone = false;
    }

    public void ReturnSeedsToInventory()
    {
        seedBagItem.Lock(false);
        seedBagItem.ReturnToPreviousParent();

        if (!seedSlot.HasItem())
        {
            ResetCannon();
            
        }
        else
        {
            // play sound to indicate did not return
        }
    }
}
