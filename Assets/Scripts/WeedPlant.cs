using System.Collections;
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
    public int secsGrowing;
    public int currentStage;
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

        IntializePlant();

    }

    public void IntializePlant()
    {
        SetStage(currentStage);

        if (isPlanted)
        {
            SetGrowthBarActive(true);
            SetWaterLevelBar(waterLevel);

            if (!fullyGrown)
            {
                StartCoroutine(GrowRoutine());
                StartCoroutine(GrowthBarUpdate());
                StartCoroutine(WaterReductionRoutine());
            }
        }
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
        secsGrowing = 0;
        harvested = false;
        trimmed = false;
        targettedForSeeding = false;
        isPlanted = true;
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
        currentStage = 5;

        seedling.SetActive(false);
        sapling.SetActive(false);
        almostGrown.SetActive(false);
        fullGrown.SetActive(false);
        fullGrownClipped.SetActive(true);
    }

    public void SetFullGrown()
    {
        currentStage = 4;

        seedling.SetActive(false);
        sapling.SetActive(false);
        almostGrown.SetActive(false);
        fullGrown.SetActive(true);
        fullGrownClipped.SetActive(false);
        SetGrowthBarActive(true);
    }

    public void SetSeedling()
    {
        currentStage = 1;
        seedling.SetActive(true);
        sapling.SetActive(false);
        fullGrown.SetActive(false);
        almostGrown.SetActive(false);
        fullGrownClipped.SetActive(false);
        SetGrowthBarActive(true);

    }

    public void SetSapling()
    {
        currentStage = 2;

        seedling.SetActive(false);
        sapling.SetActive(true);
        fullGrown.SetActive(false);
        almostGrown.SetActive(false);
        fullGrownClipped.SetActive(false);
        SetGrowthBarActive(true);
    }

    public void SetAlmostDone()
    {
        currentStage = 3;

        seedling.SetActive(false);
        sapling.SetActive(false);
        fullGrown.SetActive(false);
        almostGrown.SetActive(true);
        fullGrownClipped.SetActive(false);
        SetGrowthBarActive(true);
    }

    public void SetNone()
    {
        currentStage = 0;

        seedling.SetActive(false);
        sapling.SetActive(false);
        fullGrown.SetActive(false);
        fullGrownClipped.SetActive(false);
        SetGrowthBarActive(false);
    }

    private void SetStage(int _stage)
    {
        switch (currentStage)
        {
            case 0:
                SetNone();
                break;

            case 1:
                SetSeedling();
                break;

            case 2:
                SetSapling();
                break;

            case 3:
                SetAlmostDone();
                break;

            case 4:
                SetFullGrown();
                break;

            case 5:
                SetTrimmed();
                break;
        }
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
        // start with growthTime set to 0, set that with method, not this routine
        // add one for each second, check to see if it needs to update
        float stageTime = growTime / 3;
        do
        {
            yield return new WaitForSeconds(1);
            secsGrowing++;

            if (secsGrowing < stageTime)
                SetSeedling();
            else if (secsGrowing >= stageTime && secsGrowing < (stageTime * 2))
                SetSapling();
            else if (secsGrowing >= (stageTime * 2) && secsGrowing < growTime)
                SetAlmostDone();

        } while (secsGrowing < growTime);

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
                chickenController.SetNewDestination(chickenController.transform.position);
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
