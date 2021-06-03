using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AutoPlanterChick : MonoBehaviour
{
    [Header("Status")]
    public Transform target;

    [Header("Settings")]
    public float cannonRange;

    [Header("Setup")]
    public Transform firePoint;

    private NavMeshAgent navAgent;
    private PlanterChickHub planterHub;

    private void Start()
    {
        if (!planterHub)
            planterHub = PlanterChickHub.instance.planterChickHub.GetComponent<PlanterChickHub>();

        if (!navAgent)
            navAgent = GetComponent<NavMeshAgent>();
    }

    public void SetTarget(WeedPlant _target)
    {
        _target.targettedForSeeding = true;
        target = _target.transform;
        StartCoroutine(EngageTarget());
    }

    public IEnumerator EngageTarget()
    {
        bool targetInRange = false;
        navAgent.SetDestination(target.position);
        do
        {
            if (Vector3.Distance(transform.position, target.position) < cannonRange || target == null)
                targetInRange = true;
            yield return new WaitForSeconds(0.5f);
        } while (!targetInRange);

        if (target != null)
        {
            navAgent.SetDestination(transform.position);
            StartCoroutine(FireCannon(target));
        }
    }


    public IEnumerator FireCannon(Transform _target)
    {
        yield return new WaitForSeconds(1);

        GameObject newSeed = Instantiate(planterHub.seedProjectile);
        newSeed.transform.position = firePoint.position;
        Seed seedComp = newSeed.GetComponent<Seed>();

        seedComp.target = _target;

        WeedPlant weedPlant = _target.GetComponent<WeedPlant>();

        if (weedPlant)
            weedPlant.hasSeed = true;

        StrainProfile ammoStrain = planterHub.RequestAmmo();

        if (ammoStrain != null)
            newSeed.GetComponent<Seed>().currentStrain.SetStrain(ammoStrain);
        else
            Debug.LogWarning("Error in Auto Chick's SeedCannon, ammoStrain is null.");

        target = null;
        
    }
}
