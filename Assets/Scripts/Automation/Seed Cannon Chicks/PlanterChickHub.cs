using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanterChickHub : MonoBehaviour
{
    /*
     * NOTES
     * Generate new list of plants in range by checking for it in the same method used to move and / or place it.
     * 
     * 
     */

    [Header("Status")]
    public bool planterHubActive;
    public int availableAmmo;
    public StrainProfile currentStrain;

    [Header("Settings")]
    public float networkRadius;


    [Header("Auto Chicks")]
    public List<AutoPlanterChick> autoChicks;

    [Header("Plants")]
    public List<WeedPlant> plantsInRange;

    [Header("Setup")]
    public CanvasGroup cg;
    public GameObject seedProjectile;
    public Transform autoChicksParent;
    public GameObject radarSphere;
    public Transform inventoryParent;
    private Transform[] slots;
    public InventoryItem seedBagItem;

    public static PlanterChickHub instance;
    [HideInInspector]
    public GameObject planterChickHub;

    private void Awake()
    {
        instance = this;
        planterChickHub = gameObject;
    }

    private void Start()
    {
        if (autoChicks.Count == 0)
            UpdateAutoChickList();

        if (plantsInRange.Count == 0)
            UpdatePlantList();

        UpdateInventorySlots();

        UpdateRadarSphere();

        StartCoroutine(PlanterHubRoutine());
    }

    public IEnumerator PlanterHubRoutine()
    {
        AutoPlanterChick availableChick;
        WeedPlant availablePlant;
        do
        {
            if (availableAmmo > 0)
            {
                availableChick = GetAvailableChick();

                if (availableChick != null)
                {
                    print("AVAILABLE CHICK FOUND");
                    availablePlant = GetAvailablePlant();

                    if (availablePlant != null)
                    {
                        print("AVAILABLE PLANT FOUND");

                        availableChick.SetTarget(availablePlant);
                    }
                }

            }


            yield return new WaitForSeconds(1);
        } while (planterHubActive);
    }

    public void UpdateAutoChickList()
    {
        autoChicks.Clear();

        for (int i = 0; i < autoChicksParent.childCount; i++)
        {
            autoChicks.Add(autoChicksParent.GetChild(i).GetComponent<AutoPlanterChick>());
        }
    }

    public void UpdatePlantList()
    {
        plantsInRange.Clear();

        Vector3 radarPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(radarPos, networkRadius);
        foreach (Collider hit in colliders)
        {
            if (hit.CompareTag("Weed Plant"))
            {
                float distance = Vector3.Distance(transform.position, hit.transform.position);
                if (distance <= (networkRadius / 2))
                {
                    plantsInRange.Add(hit.GetComponent<WeedPlant>());
                }
            }
        }
    }

    public void UpdateRadarSphere()
    {
        radarSphere.transform.localScale = new Vector3(networkRadius, 1, networkRadius);

    }

    public void UpdateInventorySlots()
    {
        slots = new Transform[inventoryParent.childCount];

        for (int i = 0; i < inventoryParent.childCount; i++)
        {
            slots[i] = inventoryParent.GetChild(0);
        }

        if (seedBagItem == null)
            GetSeedBag();
    }

    public void OnSeedItemDrop(float _amt)
    {
        if (seedBagItem == null)
            GetSeedBag();
        availableAmmo += Mathf.RoundToInt(_amt);

    }

    // Call when out of ammo only
    public bool GetSeedBag()
    {
        GameObject seedBag = null;
        bool foundAmmo = false;

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].transform.childCount != 0)
            {
                seedBag = slots[i].GetChild(0).gameObject;
                foundAmmo = true;
                break;
            }
        }

        if (foundAmmo)
        {
            seedBagItem = seedBag.GetComponent<InventoryItem>();
            currentStrain.SetStrain(seedBag.GetComponent<StrainProfile>());

            seedBagItem.Lock(true);
        }
        else
        {
            seedBagItem = null;
        }

        return foundAmmo;
    }

    public AutoPlanterChick GetAvailableChick()
    {
        AutoPlanterChick availChick = null;

        for (int i = 0; i < autoChicks.Count; i++)
        {
            if (autoChicks[i].target == null)
            {
                availChick = autoChicks[i];
                break;
            }
        }

        return availChick;
    }

    public WeedPlant GetAvailablePlant()
    {
        WeedPlant availPlant = null;

        for (int i = 0; i < plantsInRange.Count; i++)
        {
            if (!plantsInRange[i].targettedForSeeding && !plantsInRange[i].isPlanted)
            {
                availPlant = plantsInRange[i];
                plantsInRange[i].targettedForSeeding = true;
                break;
            }
        }

        return availPlant;
    }

    public StrainProfile RequestAmmo()
    {
        StrainProfile ammoStrain = null;

        if (currentStrain != null)
        {
            ammoStrain = currentStrain;
            availableAmmo--;
            if (seedBagItem.AddAmount(-1))
            {
                GetSeedBag();
            }
        }


        return ammoStrain;
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

    public void ClosePanel()
    {
        SetPanelActive(false);
    }
}
