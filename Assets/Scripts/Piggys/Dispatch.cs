using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dispatch : MonoBehaviour
{
    [Header("Patrol Routes")]
    public Transform patrolRoutesParent;
    public PatrolRoute[] patrolRoutes;

    [Header("Info")]
    public int maxPerRoute;
    public int[] piggiesAssigned;

    public static Dispatch instance;
    [HideInInspector]
    public GameObject dispatch;

    private void Awake()
    {
        instance = this;
        dispatch = gameObject;
    }

    private void Start()
    {
        IntializePatrols();
    }

    private void IntializePatrols()
    {
        patrolRoutes = new PatrolRoute[patrolRoutesParent.childCount];

        for (int i = 0; i < patrolRoutes.Length; i++)
        {
            patrolRoutes[i] = patrolRoutesParent.GetChild(i).GetComponent<PatrolRoute>();
        }

        //piggiesAssigned = new int[patrolRoutesParent.childCount];
    }

    public PatrolRoute RequestNewRoute()
    {
        int rand;
        bool assignmentOkay;
        int counter = 0;
        PatrolRoute newPatrol;
        print("request new patrol started");

        do
        {
            rand = Random.Range(0, patrolRoutes.Length);
            if (patrolRoutes[rand].piggiesOnRoute < maxPerRoute)
            {
                assignmentOkay = true;
            }
            else
            {
                assignmentOkay = false;
            }
            counter++;
            if (counter == 50)
                print("counter limit reached");
        } while (!assignmentOkay && counter < 50);

        if (assignmentOkay)
        {
            newPatrol = patrolRoutes[rand];
        }
        else
        {
            print("Patrol routes full");
            newPatrol = null;
        }


        return newPatrol;
    }
}
