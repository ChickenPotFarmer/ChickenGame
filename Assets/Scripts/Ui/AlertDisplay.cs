using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AlertDisplay : MonoBehaviour
{
    [Header("Settings")]
    public float minTimeToRead;

    [Header("Setup")]
    public Animator animator;
    public TextMeshProUGUI alertTxt;

    public static AlertDisplay instance;
    [HideInInspector]
    public GameObject alerts;


    private List<string> alertMessageStack = new List<string>();
    private bool alertsRunning;

    private void Awake()
    {
        instance = this;
        alerts = gameObject;
    }

    // needs a new display for xp and rep
    public void DisplayMessage(string _message)
    {
        AddToStack(_message);
        if (!alertsRunning)
            StartCoroutine(DisplayRoutine());
    }

    private void AddToStack(string _message)
    {
        alertMessageStack.Add(_message);
    }

    private void PopAlerts()
    {
        alertMessageStack.RemoveAt(0);
    }

    IEnumerator DisplayRoutine()
    {
        alertsRunning = true;
        OpenAlert();

        do
        {
            float timeToRead = alertMessageStack[0].Length / 15;

            if (timeToRead < minTimeToRead)
                timeToRead = minTimeToRead;

            alertTxt.text = alertMessageStack[0];

            yield return new WaitForSeconds(timeToRead);
            PopAlerts();

        } while (alertMessageStack.Count != 0);
        CloseAlert();

        alertsRunning = false;

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
