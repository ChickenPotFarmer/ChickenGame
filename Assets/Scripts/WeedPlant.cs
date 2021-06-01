﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeedPlant : MonoBehaviour
{
    [Header("Info")]
    public StrainProfile currentStrain;
    public string strainID;
    public string strain;
    public float maxYield;
    public float minYield;
    public float actualYield;
    public float growTime;

    [Header("Status")]
    public bool selected;
    public bool isPlanted;
    public bool isFemale;
    public bool fullyGrown;
    public bool trimmed;
    public bool harvested;
    public bool hasSeed;

    [Header("Setup")]
    public CapsuleCollider collider;
    public CanvasGroup growthBarCg;
    public CanvasGroup harvestPanelCg;
    public Slider growthBar;
    public HarvestPanel harvestPanel;

    [Header("FX")]
    public Transform fxParent;
    public GameObject plantedFx;

    [Header("Stages")]
    public GameObject seedling;
    public GameObject sapling;
    public GameObject almostGrown;
    public GameObject fullGrown;

    private MeshRenderer debugRenderer;
    private InventoryController inventoryController;
    private ConfirmPlantPanel confirmPlantPanel;

    private void Awake()
    {
        currentStrain = GetComponent<StrainProfile>();
    }

    private void Start()
    {
        debugRenderer = GetComponent<MeshRenderer>();

        if (!inventoryController)
            inventoryController = InventoryController.instance.inventoryController.GetComponent<InventoryController>();

        if (debugRenderer.enabled)
            debugRenderer.enabled = false;

        if (!confirmPlantPanel)
            confirmPlantPanel = ConfirmPlantPanel.instance.confirmPlantPanel.GetComponent<ConfirmPlantPanel>();

    }


    public void SetConfirmPlantPanelActive(StrainProfile _strain, SeedBag _seedBag)
    {
        confirmPlantPanel.seedBag = _seedBag;
        confirmPlantPanel.potentialStrain.SetStrain(_strain);
        confirmPlantPanel.potentialPlant = this;
        confirmPlantPanel.SetConfirmPanelActive(true);

    }

    public void SetStrainProfile(StrainProfile _strain)
    {
        currentStrain.SetStrain(_strain);
    }

    public void Plant()
    {
        harvested = false;
        trimmed = false;
        StartCoroutine(GrowRoutine());
        StartCoroutine(GrowthBarUpdate());
    }

    public void SetFullGrown()
    {
        seedling.SetActive(false);
        sapling.SetActive(false);
        almostGrown.SetActive(false);
        fullGrown.SetActive(true);
        SetGrowthBarActive(true);
    }

    public void SetSeedling()
    {
        seedling.SetActive(true);
        sapling.SetActive(false);
        fullGrown.SetActive(false);
        almostGrown.SetActive(false);
        SetGrowthBarActive(true);

    }

    public void SetSapling()
    {
        seedling.SetActive(false);
        sapling.SetActive(true);
        fullGrown.SetActive(false);
        almostGrown.SetActive(false);
        SetGrowthBarActive(true);
    }

    public void SetAlmostDone()
    {
        seedling.SetActive(false);
        sapling.SetActive(false);
        fullGrown.SetActive(false);
        almostGrown.SetActive(true);
        SetGrowthBarActive(true);
    }

    public void SetNone()
    {
        seedling.SetActive(false);
        sapling.SetActive(false);
        fullGrown.SetActive(false);
        SetGrowthBarActive(false);
    }

    public void SetGrowthBarActive(bool _active)
    {
        if (_active)
        {
            growthBarCg.alpha = 1;
            growthBarCg.interactable = true;
            growthBarCg.blocksRaycasts = true;
        }
        else
        {
            growthBarCg.alpha = 0;
            growthBarCg.interactable = false;
            growthBarCg.blocksRaycasts = false;
        }
    }

    public void SetHarvestPanelActive(bool _active)
    {
        if (_active)
        {
            harvestPanelCg.alpha = 1;
            harvestPanelCg.interactable = true;
            harvestPanelCg.blocksRaycasts = true;
        }
        else
        {
            harvestPanelCg.alpha = 0;
            harvestPanelCg.interactable = false;
            harvestPanelCg.blocksRaycasts = false;
        }
    }

    public IEnumerator GrowRoutine()
    {
        isPlanted = true;
        SetSeedling();
        yield return new WaitForSeconds(growTime / 3);
        SetSapling();
        yield return new WaitForSeconds(growTime / 3);
        SetAlmostDone();
        yield return new WaitForSeconds(growTime / 3);
        SetFullGrown();
        fullyGrown = true;
    }

    public IEnumerator GrowthBarUpdate()
    {
        growthBar.maxValue = growTime + 1;
        for (int i = 0; i < growTime + 1; i++)
        {
            growthBar.value++;
            yield return new WaitForSeconds(1);
        }
    }

    public void Harvest()
    {
        if (!harvested)
        {
            harvested = true;
            
            harvestPanel.HarvestPlant(this, currentStrain);
        }
    }

    public void CloseHarvestPanel()
    {
        SetHarvestPanelActive(false);
    }

    public void ResetPlant()
    {
        CloseHarvestPanel();
        SetNone();

        fullyGrown = false;
        isPlanted = false;
        hasSeed = false;
        harvested = false;
        trimmed = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Seed Projectile"))
        {
            Seed seed = other.GetComponent<Seed>();

            if (seed.target == transform)
            {
                SetStrainProfile(seed.currentStrain);
                Plant();
                Destroy(seed.gameObject);
                Instantiate(plantedFx, fxParent);
            }
            else
            {
                print("ignore");


            }
        }
    }

}
