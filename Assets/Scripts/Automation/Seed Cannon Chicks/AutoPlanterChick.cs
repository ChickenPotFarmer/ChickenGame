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
    public float cannonFireRate;
    public float cannonVolume;

    [Header("Setup")]
    public Transform firePoint;
    [SerializeField] private NavMeshAgent navAgent;
    [SerializeField] private AudioSource cannonSound;
    private PlanterChickHub planterHub;

    private void Start()
    {
        if (!planterHub)
            planterHub = GetComponentInParent<PlanterChickHub>();
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
        do
        {
            yield return new WaitForSeconds(cannonFireRate);

            StrainProfile ammoStrain = planterHub.RequestAmmo();

            if (ammoStrain != null)
            {
                GameObject newSeed = Instantiate(planterHub.seedProjectile);
                newSeed.transform.position = firePoint.position;
                Seed seedComp = newSeed.GetComponent<Seed>();

                seedComp.target = _target;

                WeedPlant weedPlant = _target.GetComponent<WeedPlant>();

                if (weedPlant)
                    weedPlant.hasSeed = true;


                if (ammoStrain != null)
                    newSeed.GetComponent<Seed>().currentStrain.SetStrain(ammoStrain);
                else
                    Debug.LogWarning("Error in Auto Chick's SeedCannon, ammoStrain is null.");

                cannonSound.PlayOneShot(cannonSound.clip);

                target = null;
            }
        } while (target != null);

        if (planterHub)
            navAgent.SetDestination(planterHub.transform.position);    

    }
}
