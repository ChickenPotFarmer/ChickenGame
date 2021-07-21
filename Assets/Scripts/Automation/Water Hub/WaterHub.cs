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
        AutoWaterChick availableChick;
        WeedPlant availablePlant;
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
        WeedPlant availPlant = null;

        for (int i = 0; i < plantsInRange.Count; i++)
        {
            if (plantsInRange[i].isPlanted && !plantsInRange[i].targettedForWatering && !plantsInRange[i].fullyGrown && plantsInRange[i].waterLevel < minWaterLvl)
            {
                availPlant = plantsInRange[i];
                plantsInRange[i].targettedForWatering = true;
                break;
            }
        }

        return availPlant;
    }
}
