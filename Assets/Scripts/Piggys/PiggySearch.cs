using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiggySearch : MonoBehaviour
{
    [Header("Status")]
    public bool inPursuit;

    [Header("Settings")]
    public float radarDistance;
    public float patrolSpeed;
    public float chaseSpeed;

    [Header("Inventory Chicks")]
    public List<Transform> inventoryChicks;
    public List<Transform> chicksNearby;
    public Transform closestChick;

    [Header("Setup")]
    public Transform inventoryChicksParent;
    public GameObject radarSphere;

    private ChickenController chickenController;
    private InputController inputController;
    private PiggyPatrolController controller;
    private InventoryController inventoryController;

    private void Start()
    {
        if (!chickenController)
            chickenController = ChickenController.instance.chickenController.GetComponent<ChickenController>();

        if (!inputController)
            inputController = InputController.instance.inputController.GetComponent<InputController>();

        if (!inventoryController)
            inventoryController = InventoryController.instance.inventoryController.GetComponent<InventoryController>();

        if (!controller)
            controller = GetComponent<PiggyPatrolController>();

        if (controller.onPatrol)
            StartCoroutine(PatrolRoutine());
        else if (inPursuit)
            StartCoroutine(PursuitRoutine());
        StartCoroutine(PatrolRoutine());

        UpdateRadarSphere();
    }


    IEnumerator PursuitRoutine()
    {
        do
        {
            if (EnemyDetection())
                controller.navAgent.SetDestination(closestChick.position);
            else
                controller.navAgent.SetDestination(chickenController.transform.position);


            yield return new WaitForSeconds(0.5f);
        } while (inPursuit);

        controller.navAgent.speed = patrolSpeed;

    }

    // maybe replace with OnTriggerEnter
    IEnumerator PatrolRoutine()
    {
        print("patrol started");
        do
        {
            if (Vector3.Distance(transform.position, chickenController.transform.position) < radarDistance)
            {
                StartSearch();
                print("Search started");
            }
            else
            {
                print("wtf");
            }
            yield return new WaitForSeconds(0.5f);
        } while (controller.onPatrol);
        print("patrol ended");

    }

    public void RunForIt()
    {
        //GetChicks();
        StartPursuit();
    }

    public void StartPursuit()
    {
        inPursuit = true;
        controller.onPatrol = false;

        controller.navAgent.speed = chaseSpeed;
        PanicChicks();
        StartCoroutine(PursuitRoutine());

    }

    public void StartSearch()
    {
        controller.onPatrol = false;
        controller.navAgent.SetDestination(chickenController.transform.position);
        inputController.fugitive = true;
        chickenController.SetNewDestination(chickenController.transform.position);
    }

    private void GetChicks()
    {
        inventoryChicks.Clear();

        for (int i = 0; i < inventoryController.chicks.Count; i++)
        {
            inventoryChicks.Add(inventoryController.chicks[i].transform);
        }
    }

    private void PanicChicks()
    {
        GetChicks();
        for (int i = 0; i < inventoryChicks.Count; i++)
        {
            inventoryChicks[i].GetComponent<LilChickController>().panicMode = true;
        }
    }    

    public bool EnemyDetection()
    {
        bool enemyNearby = true;
        chicksNearby.Clear();
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 50);
        foreach (var hitCollider in hitColliders)
        {

            if (hitCollider.gameObject.CompareTag("Inventory Chick"))
            {
                chicksNearby.Add(hitCollider.transform);
            }


        }

        if (chicksNearby.Count > 0)
        {
            closestChick = GetClosestEnemy();
        }
        else
            enemyNearby = false;

        return enemyNearby;
    }

    public Transform GetClosestEnemy()
    {
        float closestDist = 1000;
        int closestIndex = 0;

        for (int i = 0; i < chicksNearby.Count; i++)
        {
            float dist = Vector3.Distance(transform.position, chicksNearby[i].transform.position);

            if (dist < closestDist)
            {
                closestDist = dist;
                closestIndex = i;
            }
        }

        return chicksNearby[closestIndex];
    }

    public void UpdateRadarSphere()
    {
        radarSphere.transform.localScale = new Vector3(radarDistance / 2, 1, radarDistance / 2);

    }
}
