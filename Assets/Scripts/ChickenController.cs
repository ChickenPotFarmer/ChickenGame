using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChickenController : MonoBehaviour
{
    [Header("Status")]
    public bool isTazed;

    [Header("NavMesh Agent")]
    public NavMeshAgent navAgent;
    public float runThreshold;
    public GameObject navX;

    [Header("Setup")]
    public Transform pickupSlot;
    public Animator animator;
    public Transform chickenModel;
    public GameObject tazerEffects;

    [Header("Followers")]
    public bool hasFollower;
    public LilChickController follower;

    public static ChickenController instance;
    [HideInInspector]
    public GameObject chickenController;

    private void Awake()
    {
        instance = this;
        chickenController = gameObject;
    }

    // FIND A BETTER WAY TO DO THIS
    private void Update()
    {
        if (navAgent.velocity.magnitude > runThreshold)
        {
            if (!animator.GetBool("Walk"))
                animator.SetBool("Walk", true);

            if (!navX.activeInHierarchy)
                navX.SetActive(true);
        }
        else
        {
            if (animator.GetBool("Walk"))
                animator.SetBool("Walk", false);

            if (navX.activeInHierarchy)
                navX.SetActive(false);
        }

    }

    public void TazeMeBro()
    {
        isTazed = true;
        navAgent.isStopped = true;
        //disable nav agent?
        tazerEffects.SetActive(true);

        // TazeAftermathRoutine
    }

    public void SetNewDestination(Vector3 _pos)
    {
        if (!isTazed)
        {
            navAgent.SetDestination(_pos);
            navX.transform.position = navAgent.destination;
        }
    }
}
