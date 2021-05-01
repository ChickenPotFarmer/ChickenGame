using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChickenController : MonoBehaviour
{
    [Header("NavMesh Agent")]
    public NavMeshAgent navAgent;
    public float runThreshold;
    public GameObject navX;
    public bool hasFollower;
    public LilChickController follower;

    private Animator animator;

    public static ChickenController instance;
    [HideInInspector]
    public GameObject chickenController;

    private void Awake()
    {
        instance = this;
        chickenController = gameObject;
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
    }


    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                //if (hit.collider != null)
                //{
                    //// Do Stuff
                    //string optionPicked = hit.collider.gameObject.tag;

                    //switch (optionPicked)
                    //{
                    //    case "DashBtn Battle":
                    //        SetNewDestination(hit.point);
                    //        break;
                    //}
                //}

                SetNewDestination(hit.point);
            }
        }

        if (navAgent.velocity.magnitude > runThreshold)
        {
            if (!animator.GetBool("Run"))
                animator.SetBool("Run", true);

            if (!navX.activeInHierarchy)
                navX.SetActive(true);
        }
        else
        {
            if (animator.GetBool("Run"))
                animator.SetBool("Run", false);

            if (navX.activeInHierarchy)
                navX.SetActive(false);
        }

    }

    public void SetNewDestination(Vector3 _pos)
    {
        navAgent.SetDestination(_pos);
        navX.transform.position = navAgent.destination;
    }
}
