using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterHub : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float networkRadius;
    [SerializeField] private float networkSpeed;
    [SerializeField] private float minWaterLvl;

    [Header("Setup")]
    [SerializeField] private Transform autoChicksParent;
    [SerializeField] private GameObject radarSphere;

    private List<WeedPlant> plantsInRange = new List<WeedPlant>();
    private List<AutoWaterChick> autoChicks = new List<AutoWaterChick>();

    private AutoWaterChick targetChick;

    private void Start()
    {
        IntializeHub();
    }

    private void IntializeHub()
    {
        UpdateRadarSphere();
        UpdatePlantList();
        UpdateAutoChickList();
        StartCoroutine(WaterHubRoutine());
    }

    private void UpdatePlantList()
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

    private void UpdateAutoChickList()
    {
        autoChicks.Clear();

        for (int i = 0; i < autoChicksParent.childCount; i++)
        {
            autoChicks.Add(autoChicksParent.GetChild(i).GetComponent<AutoWaterChick>());
        }
    }

    private void UpdateRadarSphere()
    {
        radarSphere.transform.localScale = new Vector3(networkRadius, 1, networkRadius);
    }

    public IEnumerator WaterHubRoutine()
    {
        targetChick = null;
        WeedPlant availablePlant;
        yield return new WaitForSeconds(Random.Range(0, 5f));

        do
        {
            //if (availableAmmo > 0)
            //{
            targetChick = GetAvailableChick();

            if (targetChick != null)
            {
                availablePlant = GetAvailablePlant();

                if (availablePlant != null)
                {
                    targetChick.SetTarget(availablePlant);
                }
            }

            //}


            yield return new WaitForSeconds(networkSpeed);
        } while (true);
    }

    public AutoWaterChick GetAvailableChick()
    {
        AutoWaterChick availChick = null;

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
        WeedPlant availPlant;
        List<WeedPlant> plantsAvailable = new List<WeedPlant>();

        for (int i = 0; i < plantsInRange.Count; i++)
        {
            if (plantsInRange[i].isPlanted && !plantsInRange[i].targettedForWatering && !plantsInRange[i].fullyGrown && plantsInRange[i].waterLevel < minWaterLvl)
            {
                plantsAvailable.Add(plantsInRange[i]);
            }
        }

        if (plantsAvailable.Count > 0)
        {
            //sort by closest plant

            //float closestDist = 1000;
            //int closestPlant = -1;
            //int tempI;

            //for (int i = 0; i < plantsAvailable.Count; i++)
            //{
            //    float dist = Vector3.Distance(targetChick.transform.position, plantsAvailable[i].transform.position);
            //    if (dist < closestDist)
            //    { 
            //        closestDist = dist;
            //        tempI = i;
            //        closestPlant = tempI;
            //    }
            //}

            //availPlant = plantsAvailable[closestPlant];
            //availPlant.targettedForWatering = true;

            // sort by lowest water lvl

            float lowestWaterLevel = 1000;
            int plantMostInNeed = -1;
            int tempI;

            for (int i = 0; i < plantsAvailable.Count; i++)
            {
                if (plantsAvailable[i].waterLevel < lowestWaterLevel)
                {
                    lowestWaterLevel = plantsAvailable[i].waterLevel;
                    tempI = i;
                    plantMostInNeed = tempI;
                }
            }

            availPlant = plantsAvailable[plantMostInNeed];
            availPlant.targettedForWatering = true;
        }
        else
        {
            availPlant = null;
        }

        return availPlant;
    }
}
