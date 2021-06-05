using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LilChickController : MonoBehaviour
{
    [Header("Settings")]
    public float stoppingDistance;
    public float enemyRadarDistance;

    [Header("Status")]
    public bool panicMode;
    private Animator animator;
    private NavMeshAgent navAgent;
    private ChickenController chickenController;
    public GameObject target;

    [Header("Piggy Detection")]
    public List<Transform> piggysNearby;
    public Transform closestPiggy;


    private void Start()
    {
        animator = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();
        navAgent.stoppingDistance = stoppingDistance;

        chickenController = ChickenController.instance.chickenController.GetComponent<ChickenController>();

        if (!chickenController.hasFollower)
        {
            chickenController.hasFollower = true;
            chickenController.follower = this;
            target = chickenController.gameObject;
        }
        else
        {
            target = chickenController.follower.gameObject;
            chickenController.follower = this;
        }
    }


    // FIND A BETTER WAY TO DO THIS;
    private void Update()
    {
        if (!panicMode)
            navAgent.SetDestination(target.transform.position);
        else
        {
            if (EnemyDetection())
            {
                Vector3 dir = (transform.position - closestPiggy.transform.position);
                dir = dir.normalized;
                dir += transform.position;
                navAgent.stoppingDistance = 0;
                navAgent.SetDestination(dir);
            }
            else
            {
                navAgent.SetDestination(target.transform.position);
                navAgent.stoppingDistance = stoppingDistance;


            }
        }

        if (navAgent.velocity.magnitude > 0)
        {
            if (!animator.GetBool("Run"))
                animator.SetBool("Run", true);
        }
        else
        {
            if (animator.GetBool("Run"))
                animator.SetBool("Run", false);
        }
    }

    public bool EnemyDetection()
    {
        bool enemyNearby = true;
        piggysNearby.Clear();
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, enemyRadarDistance);
        foreach (var hitCollider in hitColliders)
        {

            if (hitCollider.gameObject.CompareTag("Piggy"))
            {
                piggysNearby.Add(hitCollider.transform);
            }
            
            
        }

        if (piggysNearby.Count > 0)
        { 
            closestPiggy = GetClosestEnemy();
        }
        else
            enemyNearby = false;

        return enemyNearby;
    }

    public Transform GetClosestEnemy()
    {
        float closestDist = 1000;
        int closestIndex = 0;

        for (int i = 0; i < piggysNearby.Count; i++)
        {
            float dist = Vector3.Distance(transform.position, piggysNearby[i].transform.position);

            if (dist < closestDist)
            {
                closestDist = dist;
                closestIndex = i;
            }
        }

        return piggysNearby[closestIndex];
    }


}
