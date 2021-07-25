using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrimmerHub : MonoBehaviour
{
    [Header("Setup")]
    [SerializeField] private ObjectInventory objInventory;
    [SerializeField] private Transform autoChicksParent;
    [SerializeField] private Transform[] slots;
    [SerializeField] private GameObject radarSphere;
    private AutoTrimmerChick targetChick;


    [Header("Settings")]
    public float networkRadius;
    public float networkSpeed; // How fast the network pings for plants


    [Header("Auto Chicks")]
    [SerializeField] private List<AutoTrimmerChick> autoChicks; // add methods for altering chicks stats ie speed and shooting speed

    [Header("Plants")]
    [SerializeField] private List<WeedPlant> plantsInRange;

    private InventoryController inventoryController;

    private void Start()
    {
        if (!inventoryController)
            inventoryController = InventoryController.instance.inventoryController.GetComponent<InventoryController>();

        if (autoChicks.Count == 0)
            UpdateAutoChickList();

        if (plantsInRange.Count == 0)
            UpdatePlantList();

        UpdateInventorySlots();

        UpdateRadarSphere();

        StartCoroutine(TrimmerHubRoutine());
    }
    
    public bool TransferTrimmingsToHub(Transform _inventoryParent)
    {
        return inventoryController.InventoryToInventoryTransfer(_inventoryParent, objInventory.inventoryParent);
    }

    private IEnumerator TrimmerHubRoutine()
    {
        WeedPlant availablePlant;
        yield return new WaitForSeconds(Random.Range(0, 5f));

        do
        {
            targetChick = null;
            targetChick = GetAvailableChick();

            if (targetChick != null)
            {
                availablePlant = GetAvailablePlant();

                if (availablePlant != null)
                {
                    targetChick.SetTarget(availablePlant);
                }
                else
                {
                    if (targetChick.hasTrimmings && targetChick.target == null)
                    {
                        print("Could not find available plant, but chick has trimmings. Sending to hub.");
                        targetChick.ReturnToHubAndUnload();

                    }
                }
            }

            yield return new WaitForSeconds(networkSpeed);
        } while (true);
    }

    public AutoTrimmerChick GetAvailableChick()
    {
        AutoTrimmerChick availChick = null;

        for (int i = 0; i < autoChicks.Count; i++)
        {
            if (!autoChicks[i].hasTarget && !autoChicks[i].inventoryFull)
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
        List<WeedPlant> plantsAvailable = new List<WeedPlant>();

        for (int i = 0; i < plantsInRange.Count; i++)
        {
            if (plantsInRange[i].fullyGrown && !plantsInRange[i].trimmed && !plantsInRange[i].targettedForTrimming)
            {
                plantsAvailable.Add(plantsInRange[i]);
            }
        }

        if (plantsAvailable.Count > 0)
        {
            //sort by closest plant
            float closestDist = 1000;
            int closestPlant = -1;
            int tempI;

            for (int i = 0; i < plantsAvailable.Count; i++)
            {
                float dist = Vector3.Distance(targetChick.transform.position, plantsAvailable[i].transform.position);
                if (dist < closestDist)
                {
                    closestDist = dist;
                    tempI = i;
                    closestPlant = tempI;
                }
            }

            availPlant = plantsAvailable[closestPlant];
            //availPlant.targettedForTrimming = true;

        }
        else
        {
            availPlant = null;
        }

        return availPlant;
    }

    public void UpdateAutoChickList()
    {
        autoChicks.Clear();

        for (int i = 0; i < autoChicksParent.childCount; i++)
        {
            autoChicks.Add(autoChicksParent.GetChild(i).GetComponent<AutoTrimmerChick>());
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
        slots = new Transform[objInventory.inventoryParent.childCount];

        for (int i = 0; i < objInventory.inventoryParent.childCount; i++)
        {
            slots[i] = objInventory.inventoryParent.GetChild(i);
        }
    }
}
