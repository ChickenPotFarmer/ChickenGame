using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PiggyPatrolController : MonoBehaviour
{
    [Header("Status")]
    public bool onPatrol;
    public int currentWaypoint;

    [Header("Path")]
    public Transform wayPointsParent;
    public Vector3[] wayPoints;

    [Header("Setup")]
    public float runThreshold;
    public Animator animator;
    public NavMeshAgent navAgent;

    private void Start()
    {
        wayPoints = new Vector3[wayPointsParent.childCount];

        for (int i = 0; i < wayPoints.Length; i++)
        {
            wayPoints[i] = wayPointsParent.GetChild(i).transform.position;
        }

        StartPatrol();
        StartCoroutine(MovingCheck());

    }

    private void Update()
    {

        // CHANCE TO COROUTINE
        if (onPatrol)
        {
            if (Vector3.Distance(transform.position, wayPoints[currentWaypoint]) < 0.8f)
            {
                currentWaypoint++;

                if (currentWaypoint == wayPoints.Length)
                    currentWaypoint = 0;

                navAgent.SetDestination(wayPoints[currentWaypoint]);
            }
        }
    }

    // I DONT LIKE THIS
    public IEnumerator MovingCheck()
    {
        do
        {
            if (navAgent.velocity.magnitude > runThreshold)
            {
                if (!animator.GetBool("Run"))
                    animator.SetBool("Run", true);

            }
            else
            {
                if (animator.GetBool("Run"))
                    animator.SetBool("Run", false);

            }

            yield return new WaitForSeconds(0.2f);
        } while (true);
    }

    public void StartPatrol()
    {
        Vector3 closest = new Vector3(1000, 1000, 1000);

        for (int i = 0; i < wayPoints.Length; i++)
        {
            if (Vector3.Distance(transform.position, wayPoints[i]) < Vector3.Distance(transform.position, closest))
            {
                closest = wayPoints[i];
            }
        }

        int closestWaypoint = System.Array.IndexOf(wayPoints, closest);
        navAgent.SetDestination(wayPoints[closestWaypoint]);
        currentWaypoint = closestWaypoint;
        onPatrol = true;
    }
}
