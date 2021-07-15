﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PiggyPatrolController : MonoBehaviour
{
    [Header("Status")]
    public bool onPatrol;
    public bool playerSighted;
    public bool playerInRange;
    public int currentWaypoint;

    [Header("Path")]
    public PatrolRoute currentRoute;
    public Transform wayPointsParent;
    public Vector3[] wayPoints;

    [Header("Settings")]
    public float maxSpeed;
    public float stickySpeed;
    public float minWaitTime;
    public float maxWaitTime;

    [Header("Setup")]
    public GameObject susIcon;
    public GameObject pursuitIcon;
    public float runThreshold;
    public Animator animator;
    public NavMeshAgent navAgent;
    public AudioSource piggyRadio;
    public LineRenderer lineRenderer;
    public Material canSeeColor;
    public Material cannotSeeColor;

    // sus
    [Header("Sus Level Settings")]
    public float susThreshold;
    public float susLvlIncreaseRate;
    public float susCooldownRate;
    public bool isSus;
    public float susLvl;

    [Header("Pursuit Settings")]
    public bool inPursuit;
    public float searchTime;
    private float pursuitCheckLvl;
    public float pursuitCooldownRate;


    private Dispatch dispatch;
    private ChickenController chicken;
    private Rigidbody playerRb;
    private NavMeshAgent playerNavAgent;
    private Vector3 playerLastKnownPosition;
    private Vector3 playerLastKnownDirection;
    private ScreenAlert screenAlert;
    private int layerMask;


    private void Start()
    {
        if (!dispatch)
            dispatch = Dispatch.instance.dispatch.GetComponent<Dispatch>();

        if (!chicken)
            chicken = ChickenController.instance.chickenController.GetComponent<ChickenController>();

        if (!playerRb)
            playerRb = chicken.GetComponent<Rigidbody>();

        if (!playerNavAgent)
            playerNavAgent = chicken.GetComponent<NavMeshAgent>();

        if (!screenAlert)
            screenAlert = ScreenAlert.instance.screenAlert.GetComponent<ScreenAlert>();

        if (wayPointsParent != null)
            wayPoints = new Vector3[wayPointsParent.childCount];

        for (int i = 0; i < wayPoints.Length; i++)
        {
            wayPoints[i] = wayPointsParent.GetChild(i).transform.position;
        }

        layerMask = 1 << 10;

        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.

        layerMask = ~layerMask;

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

    private void Update()
    {
        if (playerInRange)
        {
            if (Physics.Linecast(transform.position + Vector3.up, chicken.chickenModel.position + Vector3.up, out RaycastHit hit, layerMask, QueryTriggerInteraction.Ignore))
            {
                Debug.DrawLine(transform.position, hit.point, Color.red);
                DrawSightLine(transform.position + (Vector3.up), playerLastKnownPosition + (Vector3.up), cannotSeeColor);
            }
            else
            {
                Debug.DrawLine(transform.position, chicken.chickenModel.position, Color.green);
                DrawSightLine(transform.position + (Vector3.up), chicken.chickenModel.position + (Vector3.up), canSeeColor);
            }
        }

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

    public void SetSticky(bool _sticky)
    {
        if (_sticky)
        {
            navAgent.speed = stickySpeed;
        }
        else
        {
            navAgent.speed = maxSpeed;
        }
    }

    IEnumerator PatrolRoutine()
    {
        // coin flip to see which direction of route to take
        bool coinFlip = Random.value > 0.5f;
        print("Returning to patrol");
        onPatrol = true;

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
        //print("sight check started");
        //// Bit shift the index of the layer (10) to get a bit mask
        //int layerMask = 1 << 10;

        //// This would cast rays only against colliders in layer 8.
        //// But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.

        //layerMask = ~layerMask;
        do
        {
            if (Physics.Linecast(transform.position + Vector3.up, chicken.chickenModel.position + Vector3.up, out RaycastHit hit, layerMask, QueryTriggerInteraction.Ignore))
            {
                Debug.DrawLine(transform.position, hit.point, Color.red);
                //DrawSightLine(transform.position, hit.point, Color.red);
                playerSighted = false;

                screenAlert.SetSus(false);
                susIcon.SetActive(false);
            }
            else
            {
                playerSighted = true;
                Debug.DrawLine(transform.position, chicken.chickenModel.position, Color.green);
                //DrawSightLine(transform.position, chicken.chickenModel.position, Color.green);
                PlayerSighted();
            }

            yield return new WaitForSeconds(0.1f);
        } while (playerInRange);

        if (!playerInRange)
            screenAlert.SetSus(false);

        lineRenderer.positionCount = 0;
        playerSighted = false;

    }

    private void DrawSightLine(Vector3 _start, Vector3 _end, Material _color)
    {
        lineRenderer.positionCount = 2;

        lineRenderer.SetPositions(new Vector3[] { _start, _end });
        lineRenderer.material = _color;
    }

    private void PlayerSighted()
    {
        //print("Player Sighted");
        playerLastKnownPosition = chicken.chickenModel.position;
        playerLastKnownDirection = playerNavAgent.velocity.normalized;
        screenAlert.SetSus(true);
        susIcon.SetActive(true);


        if (!isSus && !inPursuit)
            StartCoroutine(SusRoutine()); 

    }

    private IEnumerator SusRoutine()
    {
        print("Sus routine Started");

        isSus = true;
        susLvl = 0;

        do
        {
            susLvl += susLvlIncreaseRate;

            if (susLvl >= susThreshold && !inPursuit)
            {
                StartCoroutine(PursuitRoutine());
                break;
            }

            yield return new WaitForSeconds(0.1f);
        } while (playerSighted);

        if (susLvl <= 0)
        { 
            isSus = false;
            susLvl = 0;
        }
        else
            StartCoroutine(SusCooldown());
    }

    private IEnumerator SusCooldown()
    {
        print("Sus cooldown Started");

        do
        {
            susLvl -= susCooldownRate;
            yield return new WaitForSeconds(0.1f);

        } while (susLvl > 0);

        if (susLvl <= 0)
        {
            isSus = false;
            susLvl = 0;
            screenAlert.SetSus(false);
            susIcon.SetActive(false);

        }
    }

    private IEnumerator PursuitRoutine()
    {
        inPursuit = true;
        onPatrol = false;
        StopCoroutine(PatrolRoutine());
        pursuitCheckLvl = 1;
        screenAlert.SetPursuit(true);
        pursuitIcon.SetActive(true);

        print("Pursuit Started");

        do
        {
            if (playerSighted)
            {
                pursuitCheckLvl = 1;
                PlayerSighted();
                navAgent.SetDestination(playerLastKnownPosition);

            }
            else
            {

                if (Vector3.Distance(transform.position, playerLastKnownPosition ) < 1.5f)
                {
                    navAgent.SetDestination(transform.position + (playerLastKnownDirection * 4));
                    playerLastKnownPosition = navAgent.destination;
                    Debug.DrawLine(transform.position, transform.position + (playerLastKnownDirection * 4), Color.blue);

                    print("Headed in last known player direction.");
                }
                else
                {
                    navAgent.SetDestination(playerLastKnownPosition);
                    print("Headed to last known player positon.");

                }

                pursuitCheckLvl -= pursuitCooldownRate;

                if (pursuitCheckLvl <= 0)
                {
                    inPursuit = false;
                    screenAlert.SetPursuit(false);
                    pursuitIcon.SetActive(false);
                    break;
                }
            }

            yield return new WaitForSeconds(0.1f);

        } while (inPursuit);
        inPursuit = false;
        screenAlert.SetPursuit(false);
        pursuitIcon.SetActive(false);

        RequestNewPatrol();

    }

    //private void ReturnToPatrol()
    //{
    //    StartCoroutine(PatrolRoutine());

    //}

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
