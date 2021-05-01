using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LilChickController : MonoBehaviour
{
    private Animator animator;
    private NavMeshAgent navAgent;
    private ChickenController chickenController;
    public GameObject target;


    private void Start()
    {
        animator = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();

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

    private void Update()
    {
        navAgent.SetDestination(target.transform.position);

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
}
