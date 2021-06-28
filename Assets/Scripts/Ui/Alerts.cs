using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alerts : MonoBehaviour
{
    [Header("Setup")]
    public Animator animator;

    public static Alerts instance;
    [HideInInspector]
    public GameObject alerts;

    private void Awake()
    {
        instance = this;
        alerts = gameObject;
    }

    private void Update()
    {
        if (Input.GetKeyDown("7"))
            OpenAlert();
        else if (Input.GetKeyDown("8"))
            CloseAlert();
    }

    public void OpenAlert()
    {
        animator.SetTrigger("Open");
    }

    public void CloseAlert()
    {
        animator.SetTrigger("Close");

    }
}
