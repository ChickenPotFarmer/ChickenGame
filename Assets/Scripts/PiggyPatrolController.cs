using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PiggyPatrolController : MonoBehaviour
{
    [Header("Status")]
    public bool onPatrol;
    public bool playerInRange;
    public int currentWaypoint;

    [Header("Path")]
    public PatrolRoute currentRoute;
    public Transform wayPointsParent;
    public Vector3[] wayPoints;

    [Header("Settings")]
    public float minWaitTime;
    public float maxWaitTime;

    [Header("Setup")]
    public float runThreshold;
    public Animator animator;
    public NavMeshAgent navAgent;
    public AudioSource piggyRadio;

    private Dispatch dispatch;
    private ChickenController chicken;
    private Rigidbody playerRb;
    private Vector3 playerLastKnownPosition;
    private Vector3 playerLastKnownDirection;


    private void Start()
    {
        if (!dispatch)
            dispatch = Dispatch.instance.dispatch.GetComponent<Dispatch>();

        if (!chicken)
            chicken = ChickenController.instance.chickenController.GetComponent<ChickenController>();

        if (!playerRb)
            playerRb = chicken.GetComponent<Rigidbody>();

        if (wayPointsParent != null)
            wayPoints = new Vector3[wayPointsParent.childCount];

        for (int i = 0; i < wayPoints.Length; i++)
        {
            wayPoints[i] = wayPointsParent.GetChild(i).transform.position;
        }

        if (currentRoute != null)
            StartPatrol();
        else
            RequestNewPatrol();

        StartCoroutine(MovingCheck());
        StartCoroutine(PatrolRoutine());

        // this is stupid
        float rand = Random.Range(0, 15.1f);
        Invoke("StartRadio", rand);

    }

    //private void Update()
    //{

    //    if (Input.GetKeyDown(KeyCode.Space))
    //        RequestNewPatrol();

    //    // CHANCE TO COROUTINE
    //    //if (onPatrol)
    //    //{
    //    //    if (Vector3.Distance(transform.position, wayPoints[currentWaypoint]) < 0.8f)
    //    //    {
    //    //        currentWaypoint++;

    //    //        if (currentWaypoint == wayPoints.Length)
    //    //            currentWaypoint = 0;

    //    //        navAgent.SetDestination(wayPoints[currentWaypoint]);
    //    //    }
    //    //}
    //}

    private void StartRadio()
    {
        piggyRadio.Play();
    }


    IEnumerator PatrolRoutine()
    {
        // coin flip to see which direction of route to take
        bool coinFlip = Random.value > 0.5f;

        do
        {
            if (coinFlip)
            {
                if (Vector3.Distance(transform.position, wayPoints[currentWaypoint]) < 0.8f)
                {
                    currentWaypoint++;

                    if (currentWaypoint == wayPoints.Length)
                        currentWaypoint = 0;

                    navAgent.SetDestination(wayPoints[currentWaypoint]);
                }
            }
            else
            {
                if (Vector3.Distance(transform.position, wayPoints[currentWaypoint]) < 0.8f)
                {
                    currentWaypoint--;

                    if (currentWaypoint == -1)
                        currentWaypoint = wayPoints.Length - 1;

                    navAgent.SetDestination(wayPoints[currentWaypoint]);
                }
            }
            yield return new WaitForSeconds(0.1f);
        } while (onPatrol);
    }
    // I DONT LIKE THIS
    private IEnumerator MovingCheck()
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

    private IEnumerator SightCheck()
    {
        print("sight check started");
        // Bit shift the index of the layer (10) to get a bit mask
        int layerMask = 1 << 10;

        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        layerMask = ~layerMask;
        do
        {
            if (Physics.Linecast(transform.position, chicken.chickenModel.position, out RaycastHit hit, layerMask, QueryTriggerInteraction.Ignore))
            {
                Debug.Log("blocked");
                Debug.DrawLine(transform.position, hit.point, Color.red);
            }
            else
            {
                print("Not Blocked");
                Debug.DrawLine(transform.position, chicken.chickenModel.position, Color.green);
                playerLastKnownPosition = chicken.chickenModel.position;
                playerLastKnownDirection = chicken.chickenModel.position;
            }

            yield return new WaitForSeconds(0.1f);
        } while (playerInRange);
    }

    public void RequestNewPatrol()
    {
        StartCoroutine(NewPatrolRoutine());
    }

    IEnumerator NewPatrolRoutine()
    {
        float rand = Random.Range(minWaitTime, maxWaitTime);
        navAgent.SetDestination(transform.position);
        yield return new WaitForSeconds(rand);

        // Get new route
        if (currentRoute != null)
        {
            currentRoute.piggiesOnRoute--;
        }
        currentRoute = dispatch.RequestNewRoute();


        if (currentRoute != null)
        {
            SetPatrol(currentRoute.waypointsParent);
            currentRoute.piggiesOnRoute++;

        }
        else
        {
            print("Failed to get new patrol. Waiting 10 secs before trying again.");
            yield return new WaitForSeconds(10);
            RequestNewPatrol();
        }
    }

    public void SetPatrol(Transform _waypointsParent)
    {
        if (_waypointsParent != null)
        {
            wayPoints = new Vector3[_waypointsParent.childCount];

            for (int i = 0; i < wayPoints.Length; i++)
            {
                wayPoints[i] = _waypointsParent.GetChild(i).transform.position;
            }

            StartPatrol();
        }
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!playerInRange)
            {
                playerInRange = true;
                StartCoroutine(SightCheck());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (playerInRange)
            {
                playerInRange = false;
            }
        }
    }
}
