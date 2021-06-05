using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiggySearch : MonoBehaviour
{
    [Header("Status")]
    public bool inPursuit;

    [Header("Settings")]
    public float patrolSpeed;
    public float chaseSpeed;

    [Header("Inventory Chicks")]
    public List<Transform> inventoryChicks;
    public List<Transform> chicksNearby;
    public Transform closestChick;

    [Header("Setup")]
    public Transform inventoryChicksParent;
    

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

    public void RunForIt()
    {
        GetChicks();
        StartPursuit();
    }

    public void StartPursuit()
    {
        inPursuit = true;
        controller.navAgent.speed = chaseSpeed;
        StartCoroutine(PursuitRoutine());

    }

    private void GetChicks()
    {
        inventoryChicks.Clear();

        for (int i = 0; i < inventoryController.chicks.Count; i++)
        {
            inventoryChicks.Add(inventoryController.chicks[i].transform);
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
}
