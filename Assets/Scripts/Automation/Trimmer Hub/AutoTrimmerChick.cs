using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AutoTrimmerChick : MonoBehaviour
{
    [Header("Status")]
    public WeedPlant target;
    public bool hasTrimmings;
    public bool inventoryFull;
    public bool hasTarget;

    [Header("Settings")]
    public int minTrimmings;
    public int maxTrimmings;

    [Header("Setup")]
    [SerializeField] private NavMeshAgent navAgent;
    [SerializeField] private ObjectInventory objInventory;
    [SerializeField] private GameObject trimmerModel;
    [SerializeField] private GameObject trimmingsPrefab;
    [SerializeField] private GameObject uiTrimmingsPrefab;

    private TrimmerHub trimmerHub;
    private InventoryController inventoryController;

    private void Start()
    {
        if (!inventoryController)
            inventoryController = InventoryController.instance.inventoryController.GetComponent<InventoryController>();

        if (!trimmerHub)
            trimmerHub = GetComponentInParent<TrimmerHub>();
    }

    public void SetTarget(WeedPlant _target)
    {
        target = _target;
        target.targettedForTrimming = true;
        hasTarget = true;
        StartCoroutine(TrimTargetRoutine());
    }

    private IEnumerator TrimTargetRoutine()
    {
        GameObject newTrimmings;
        InventoryItem trimmingsItem;
        int trimmings;

        if (target != null)
        {
            // Go to plant
            navAgent.SetDestination(target.transform.position);
            do
            {
                if (Vector3.Distance(transform.position, target.transform.position) < 5)
                {
                    target.Trim();

                    navAgent.SetDestination(transform.position);
                    print("PLANT TRIM ATTEMPTED");

                    trimmings = Random.Range(minTrimmings, maxTrimmings + 1);

                    //StartCoroutine(SpawnRoutine(trimmings));

                    newTrimmings = Instantiate(uiTrimmingsPrefab);
                    trimmingsItem = newTrimmings.GetComponent<InventoryItem>();
                    trimmingsItem.SetAmount(trimmings);
                    inventoryFull = inventoryController.ReturnToInventory(trimmingsItem, objInventory.inventoryParent);

                    //flip it because it returns if there was no remainder
                    //inventoryFull = !inventoryFull;

                    Xp.TrimPlant();
                    hasTrimmings = true;
                    target = null;
                    hasTarget = false;
                }
                yield return new WaitForSeconds(1);
            } while (target != null);
        }
        if (inventoryFull)
        {
            print("Auto chick inventory full returning to trimmer hub");
            StartCoroutine(UnloadTrimmingsRoutine());
        }
        //else
            //target = null;

    }

    public void ReturnToHubAndUnload()
    {
        StartCoroutine(UnloadTrimmingsRoutine());

    }

    private IEnumerator UnloadTrimmingsRoutine()
    {
        // Return to hub
        navAgent.SetDestination(trimmerHub.transform.position);
        if (Vector3.Distance(transform.position, trimmerHub.transform.position) < 5)
        {
            navAgent.SetDestination(transform.position);
        }
        yield return new WaitForSeconds(1);

        bool noRemainder;

        do
        {
            noRemainder = trimmerHub.TransferTrimmingsToHub(objInventory.inventoryParent);

            yield return new WaitForSeconds(2);
        } while (!noRemainder);

        hasTrimmings = false;
        inventoryFull = false;
        //target = null;
    }

    IEnumerator SpawnRoutine(int _amt)
    {
        GameObject trim;
        Vector3 pos = target.transform.position;
        for (int i = 0; i < _amt; i++)
        {
            yield return new WaitForSeconds(0.12f);
            trim = Instantiate(trimmingsPrefab);
            trim.GetComponent<PickUpObject>().targetOverride = gameObject;
            trim.transform.position = pos;
        }
    }
}
