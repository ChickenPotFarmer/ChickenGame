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

    private void Awake()
    {
        instance = this;
        alerts = gameObject;
    }

    public void DisplayMessage(string _message)
    {
        StartCoroutine(DisplayRoutine(_message));
    }

    IEnumerator DisplayRoutine(string _message)
    {
        float timeToRead = _message.Length / 15;

        if (timeToRead < minTimeToRead)
            timeToRead = minTimeToRead;

        print("Wait time: " + timeToRead + "Length: " + _message.Length);

        alertTxt.text = _message;
        OpenAlert();

        yield return new WaitForSeconds(timeToRead);
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
