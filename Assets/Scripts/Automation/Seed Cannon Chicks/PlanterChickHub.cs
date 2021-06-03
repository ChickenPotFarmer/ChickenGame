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
    public List<WeedPlant> plantTargets;

    [Header("Settings")]
    public float networkRadius;

    [Header("Auto Chicks")]
    public List<AutoPlanterChick> autoChicks;

    [Header("Plants")]
    public List<WeedPlant> plantsInRange;

    [Header("Setup")]
    public Transform autoChicksParent;
    public GameObject radarSphere;

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

        UpdateRadarSphere();
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

    public void OnSeedItemDrop()
    {

    }
}
