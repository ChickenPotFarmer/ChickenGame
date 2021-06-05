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
    public bool targettedForHarvest;
    public bool targettedForSeeding;
    public bool selected;
    public bool isPlanted;
    public bool isFemale;
    public bool fullyGrown;
    public bool trimmed;
    public bool harvested;
    public bool hasSeed;

    [Header("Watering")]
    public float startingWaterLevel;
    public float waterLevel; //out of 100
    public float dryOutRate;

    [Header("Setup")]
    public ParticleSystem waterParticles;
    public CapsuleCollider plantCollider;
    public CanvasGroup growthBarCg;
    public CanvasGroup harvestPanelCg;
    public Slider growthBar;
    public Slider waterBar;
    public HarvestPanel harvestPanel;

    [Header("FX")]
    public Transform fxParent;
    public GameObject plantedFx;

    [Header("Stages")]
    public GameObject seedling;
    public GameObject sapling;
    public GameObject almostGrown;
    public GameObject fullGrown;
    public GameObject fullGrownClipped;

    private MeshRenderer debugRenderer;
    private InventoryController inventoryController;
    private ConfirmPlantPanel confirmPlantPanel;
    private ChickenController chickenController;

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

        if (!chickenController)
            chickenController = ChickenController.instance.chickenController.GetComponent<ChickenController>();

        waterParticles.Stop();

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
        targettedForSeeding = false;
        waterLevel = startingWaterLevel;
        SetWaterLevelBar(waterLevel);
        StartCoroutine(GrowRoutine());
        StartCoroutine(GrowthBarUpdate());
        StartCoroutine(WaterReductionRoutine());

    }

    public void Trim()
    {
        trimmed = true;
        SetTrimmed();
    }

    public void Water()
    {
        if (waterLevel != 100)
        {
            waterLevel = 100;

            StartCoroutine(WaterRoutine());
        }
        else
        {
            print("Plant water level is full.");
        }

    }

    IEnumerator WaterRoutine()
    {
        waterParticles.Play();

        yield return new WaitForSeconds(2f);
        SetWaterLevelBar(waterLevel);

        yield return new WaitForSeconds(1f);
        waterParticles.Stop();

    }

    public void SetWaterLevelBar(float _amt)
    {
        waterBar.value = _amt;
    }

    IEnumerator WaterReductionRoutine()
    {
        do
        {
            waterLevel--;

            if (waterLevel < 0)
                waterLevel = 0;

            SetWaterLevelBar(waterLevel);

            yield return new WaitForSeconds(dryOutRate);
        } while (isPlanted);

    }

    public void SetTrimmed()
    {
        seedling.SetActive(false);
        sapling.SetActive(false);
        almostGrown.SetActive(false);
        fullGrown.SetActive(false);
        fullGrownClipped.SetActive(true);
    }

    public void SetFullGrown()
    {
        seedling.SetActive(false);
        sapling.SetActive(false);
        almostGrown.SetActive(false);
        fullGrown.SetActive(true);
        fullGrownClipped.SetActive(false);
        SetGrowthBarActive(true);
    }

    public void SetSeedling()
    {
        seedling.SetActive(true);
        sapling.SetActive(false);
        fullGrown.SetActive(false);
        almostGrown.SetActive(false);
        fullGrownClipped.SetActive(false);
        SetGrowthBarActive(true);

    }

    public void SetSapling()
    {
        seedling.SetActive(false);
        sapling.SetActive(true);
        fullGrown.SetActive(false);
        almostGrown.SetActive(false);
        fullGrownClipped.SetActive(false);
        SetGrowthBarActive(true);
    }

    public void SetAlmostDone()
    {
        seedling.SetActive(false);
        sapling.SetActive(false);
        fullGrown.SetActive(false);
        almostGrown.SetActive(true);
        fullGrownClipped.SetActive(false);
        SetGrowthBarActive(true);
    }

    public void SetNone()
    {
        seedling.SetActive(false);
        sapling.SetActive(false);
        fullGrown.SetActive(false);
        fullGrownClipped.SetActive(false);
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

    // set timer so it doesnt harvest if player clicks somewhere else and cancels
    public void TargetForHarvest()
    {
        StartCoroutine(CheckChickenDistanceRoutine());
    }

    IEnumerator CheckChickenDistanceRoutine()
    {
        float endCheck = Time.time + 5;
        do
        {
            if (Vector3.Distance(transform.position, chickenController.transform.position) < 4 && !harvested)
            {
                harvested = true;

                harvestPanel.HarvestPlant(this, currentStrain);
            }
            yield return new WaitForSeconds(0.2f);
        } while (Time.time < endCheck && !harvested);


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
