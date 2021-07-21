using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AutoWaterChick : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float chickRange;
    [SerializeField] private float cannonFireRate;
    [SerializeField] private float stuckTimer;

    [Header("Status")]
    public WeedPlant target;

    [Header("Setup")]
    [SerializeField] private NavMeshAgent navAgent;

    private WaterHub waterHub;

    private void Start()
    {
        if (!waterHub)
            waterHub = GetComponentInParent<WaterHub>();
    }

    public void SetTarget(WeedPlant _target)
    {
        _target.targettedForSeeding = true;
        target = _target;
        StartCoroutine(EngageTarget());
    }

    public IEnumerator EngageTarget()
    {
        bool targetInRange = false;
        float timer = 0;
        do
        {
            navAgent.SetDestination(target.transform.position);
            do
            {
                if (Vector3.Distance(transform.position, target.transform.position) < chickRange || target == null)
                    targetInRange = true;
                yield return new WaitForSeconds(0.5f);

                if (target == null)
                    break;

            } while (!targetInRange);

            if (target != null)
            {
                target.Water();
                target = null;
                navAgent.SetDestination(transform.position);
            }
            timer += 0.5f;

            if (timer >= stuckTimer)
                target = null;
        } while (target != null);

        if (waterHub)
            navAgent.SetDestination(waterHub.transform.position);
    }

    
}
