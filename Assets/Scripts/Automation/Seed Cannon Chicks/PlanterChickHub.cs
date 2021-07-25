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
    public bool randomlySelectSeeds;
    //public int availableAmmo;

    [Header("Settings")]
    public float networkRadius;
    public float networkSpeed; // How fast the network pings for plants


    [Header("Auto Chicks")]
    public List<AutoPlanterChick> autoChicks; // add methods for altering chicks stats ie speed and shooting speed

    [Header("Plants")]
    public List<WeedPlant> plantsInRange;

    [Header("Setup")]
    public CanvasGroup cg;
    public GameObject seedProjectile;
    public Transform autoChicksParent;
    public GameObject radarSphere;
    public Transform inventoryParent;
    public Transform[] slots;
    //public InventoryItem seedBagItem;

    private List<InventoryItem> seedBags = new List<InventoryItem>();

    private void Start()
    {
        if (autoChicks.Count == 0)
            UpdateAutoChickList();

        if (plantsInRange.Count == 0)
            UpdatePlantList();

        UpdateInventorySlots();

        UpdateRadarSphere();

        StartCoroutine(InventoryUpdateRoutine());

        StartCoroutine(PlanterHubRoutine());
    }

    private IEnumerator InventoryUpdateRoutine()
    {
        do
        {
            UpdateSeedBagList();
            yield return new WaitForSeconds(0.1f);
        } while (true);
    }

    public IEnumerator PlanterHubRoutine()
    {
        AutoPlanterChick availableChick;
        WeedPlant availablePlant;
        yield return new WaitForSeconds(Random.Range(0, 5f));

        do
        {
            //if (availableAmmo > 0)
            //{
                availableChick = GetAvailableChick();

                if (availableChick != null)
                {
                    availablePlant = GetAvailablePlant();

                    if (availablePlant != null)
                    {
                        availableChick.SetTarget(availablePlant);
                    }
                }

            //}


            yield return new WaitForSeconds(networkSpeed);
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

        if (plantsInRange.Count > 1)
        {
            WeedPlant tempPlant;
            for (int i = 0; i < plantsInRange.Count - 1; i++)
            {
                if (Vector3.Distance(transform.position, plantsInRange[i + 1].transform.position) < Vector3.Distance(transform.position, plantsInRange[i].transform.position))
                {
                    tempPlant = plantsInRange[i];
                    plantsInRange[i] = plantsInRange[i + 1];
                    plantsInRange[i + 1] = tempPlant;
                    i = -1;
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
            slots[i] = inventoryParent.GetChild(i);
        }
    }

    private void UpdateSeedBagList()
    {
        seedBags.Clear();
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].childCount > 0)
            {
                seedBags.Add(slots[i].GetChild(0).GetComponent<InventoryItem>());
            }
        }
    }

    public void OnSeedItemDrop()
    {
        // refresh seedbag list
        UpdateSeedBagList();


        //GetSeedBag();
        //availableAmmo += Mathf.RoundToInt(_amt);

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
        int rand;
        if (seedBags.Count != 0)
        {
            if (randomlySelectSeeds)
            {
                rand = Random.Range(0, seedBags.Count);
            }
            else
            {
                rand = 0;
            }
            ammoStrain = seedBags[rand].GetComponent<StrainProfile>();
            seedBags[rand].AddAmount(-1);

            if (seedBags[rand].amount == 0)
                seedBags.RemoveAt(rand);
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
