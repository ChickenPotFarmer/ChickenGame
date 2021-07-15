using System.Collections;
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
    private float distanceToPlayer;

    [Header("Path")]
    public PatrolRoute currentRoute;
    public Transform wayPointsParent;
    public Vector3[] wayPoints;

    [Header("Settings")]
    [SerializeField]
    private float attackDistance;
    [SerializeField]
    private float attackRate;
    [SerializeField]
    private float attackAccuracy;
    [SerializeField]
    private float stickyTime;
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
    [SerializeField]
    private GameObject holoChicken;

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
    private Quaternion playerLastKnownRotation;
    private ScreenAlert screenAlert;
    private int layerMask;
    private bool isAttacking;
    private bool isSticky;


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
        StartCoroutine(StuckCheck());

        // this is stupid
        float rand = Random.Range(0, 15.1f);
        Invoke("StartRadio", rand);

    }

    private void Update()
    {
        if (playerInRange)
        {
            if (Physics.Linecast(transform.position + Vector3.up, chicken.chickenModel.position + Vector3.up, out RaycastHit hit, layerMask, QueryTriggerInteraction.Ignore) || isSticky)
            {
                Debug.DrawLine(transform.position, hit.point, Color.red);
                DrawSightLine(transform.position + (Vector3.up), playerLastKnownPosition + (Vector3.up / 2), cannotSeeColor);
                if (!holoChicken.activeInHierarchy)
                    holoChicken.SetActive(true);
                holoChicken.transform.position = playerLastKnownPosition;
                holoChicken.transform.rotation = playerLastKnownRotation;
            }
            else
            {
                Debug.DrawLine(transform.position, chicken.chickenModel.position, Color.green);
                DrawSightLine(transform.position + (Vector3.up), chicken.chickenModel.position + (Vector3.up), canSeeColor);
                if (holoChicken.activeInHierarchy)
                    holoChicken.SetActive(false);

            }
        }
        else
        {
            if (holoChicken.activeInHierarchy)
                holoChicken.SetActive(false);
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

    public void SetSticky()
    {
        if (!isSticky)
            StartCoroutine(StickyRoutine());
        else
        {
            stickyI = 0;
        }
    }

    private int stickyI;

    private IEnumerator StickyRoutine()
    {
        isSticky = true;
        navAgent.speed = stickySpeed;
        for (stickyI = 0; stickyI < stickyTime; stickyI++)
        {
            yield return new WaitForSeconds(1);
        }

        if (inPursuit)
            navAgent.speed = maxSpeed;
            else
            navAgent.speed = maxSpeed * 0.7f;

        isSticky = false;


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
                //Debug.DrawLine(transform.position, hit.point, Color.red);
                //DrawSightLine(transform.position, hit.point, Color.red);
                playerSighted = false;

                screenAlert.SetSus(false);
                susIcon.SetActive(false);
            }
            else
            {
                if (!isSticky)
                {
                    playerSighted = true;
                    //Debug.DrawLine(transform.position, chicken.chickenModel.position, Color.green);
                    //DrawSightLine(transform.position, chicken.chickenModel.position, Color.green);
                    PlayerSighted();
                }
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
        playerLastKnownRotation = chicken.chickenModel.rotation;
        playerLastKnownDirection = playerNavAgent.velocity.normalized;
        distanceToPlayer = Vector3.Distance(transform.position, chicken.transform.position);

        screenAlert.SetSus(true);
        susIcon.SetActive(true);


        if (!isSus && !inPursuit)
            StartCoroutine(SusRoutine()); 

        if (distanceToPlayer <= attackDistance && !isAttacking && !isSticky)
        {
            StartCoroutine(AttackRoutine());
        }

    }

    private IEnumerator AttackRoutine()
    {
        isAttacking = true;

        do
        {
            if (Random.value <= attackAccuracy)
            {
                //fire tazer
                chicken.TazeMeBro();
            }
            yield return new WaitForSeconds(attackRate);
        } while (distanceToPlayer <= attackDistance);

        isAttacking = false;
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
        } while (playerSighted && !chicken.isTazed);

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
        navAgent.speed = maxSpeed;
        screenAlert.SetPursuit(true);
        pursuitIcon.SetActive(true);

        print("Pursuit Started");

        do
        {
            if (!isSticky)
            {
                if (playerSighted)
                {
                    pursuitCheckLvl = 1;
                    PlayerSighted();
                    navAgent.SetDestination(playerLastKnownPosition);

                }
                else
                {

                    if (Vector3.Distance(transform.position, playerLastKnownPosition) < 1.5f)
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
            }
            else
            {
                navAgent.SetDestination(transform.position + new Vector3(Random.Range(-20, 20), 0, Random.Range(-20, 20)));
            }

            yield return new WaitForSeconds(0.1f);

        } while (inPursuit && !chicken.isTazed);
        inPursuit = false;
        screenAlert.SetPursuit(false);
        pursuitIcon.SetActive(false);

        RequestNewPatrol();

    }

    //private void ReturnToPatrol()
    //{
    //    StartCoroutine(PatrolRoutine());

    //}

    IEnumerator StuckCheck()
    {
        int secsStuck = 0;
        Vector3 prevPos = transform.position;
        do
        {
            yield return new WaitForSeconds(1);

            if (Vector3.Distance(transform.position, prevPos) <= 1)
            {
                secsStuck++;
            }
            else
                secsStuck = 0;

            if (secsStuck >= 10)
            {
                RequestNewPatrol();
            }
            prevPos = transform.position;

        } while (true);
    }

    public void RequestNewPatrol()
    {
        if (!chicken.isTazed)
            StartCoroutine(NewPatrolRoutine());
        else
            navAgent.SetDestination(transform.position);
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
        navAgent.speed = maxSpeed * 0.7f;
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
