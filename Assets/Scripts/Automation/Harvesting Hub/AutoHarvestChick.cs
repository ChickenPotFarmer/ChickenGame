using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AutoHarvestChick : MonoBehaviour
{
    [Header("Status")]
    public WeedPlant target;
    public bool hasWeed;
    public bool inventoryFull;
    public bool hasTarget;

    [Header("Setup")]
    [SerializeField] private NavMeshAgent navAgent;
    [SerializeField] private ObjectInventory objInventory;

    private HarvestHub harvestHub;
    private InventoryController inventoryController;

    private void Start()
    {
        if (!inventoryController)
            inventoryController = InventoryController.instance.inventoryController.GetComponent<InventoryController>();

        if (!harvestHub)
            harvestHub = GetComponentInParent<HarvestHub>();
    }

    public void SetTarget(WeedPlant _target)
    {
        target = _target;
        target.targettedForHarvest = true;
        hasTarget = true;
        StartCoroutine(TargetRoutine());
    }

    private IEnumerator TargetRoutine()
    {
        bool noRemainder;
        if (target != null)
        {
            // Go to plant
            navAgent.SetDestination(target.transform.position);
            do
            {
                if (Vector3.Distance(transform.position, target.transform.position) < 5)
                {
                    target.Harvest(false);
                    noRemainder = inventoryController.InventoryToInventoryTransfer(target.harvestPanel.slotsParent, objInventory.inventoryParent);

                    if (!noRemainder)
                        inventoryFull = true;

                    target.harvestPanel.ClearInventory();

                    Xp.TrimPlant();
                    hasWeed = true;
                    target = null;
                    hasTarget = false;
                }
                yield return new WaitForSeconds(1);
            } while (target != null);
        }
        if (inventoryFull)
        {
            print("Auto chick inventory full returning to trimmer hub");
            StartCoroutine(UnloadRoutine());
        }
        //else
        //target = null;

    }

    public void ReturnToHubAndUnload()
    {
        StartCoroutine(UnloadRoutine());

    }

    private IEnumerator UnloadRoutine()
    {
        // Return to hub
        navAgent.SetDestination(harvestHub.transform.position);
        if (Vector3.Distance(transform.position, harvestHub.transform.position) < 5)
        {
            navAgent.SetDestination(transform.position);
        }
        yield return new WaitForSeconds(1);

        bool noRemainder;

        do
        {
            noRemainder = harvestHub.TransferToHub(objInventory.inventoryParent);
            yield return new WaitForSeconds(2);
        } while (!noRemainder);

        hasWeed = false;
        inventoryFull = false;
        //target = null;
    }
}
