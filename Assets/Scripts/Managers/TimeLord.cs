using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimeLord : MonoBehaviour
{
    [Header("Status")]
    public int currentMonth;
    public int currentYear;

    [Header("Settings")]
    public int secsPerMonth;

    [Header("Setup")]
    public string[] months;
    public TextMeshProUGUI dateTxt;
    public Slider monthSlider;

    public static TimeLord instance;
    [HideInInspector]
    public GameObject timeLord;

    private EmailGenerator emailGenerator;

    private void Awake()
    {
        instance = this;
        timeLord = gameObject;
    }


    private void Start()
    {
        if (!emailGenerator)
            emailGenerator = EmailGenerator.instance.emailGenerator.GetComponent<EmailGenerator>();

        DisplayDate();
        StartCoroutine(TimeRoutine());
    }

    IEnumerator TimeRoutine()
    {
        int timePerMonth = secsPerMonth * 10;
        monthSlider.maxValue = secsPerMonth;

        do
        {
            NewMonthTasks();

            for (int i = 0; i < timePerMonth; i++)
            {
                monthSlider.value += 0.1f;
                yield return new WaitForSeconds(0.1f);

            }

            monthSlider.value = 0;
            currentMonth++;
            
            if (currentMonth > 11)
            {
                currentMonth = 0;
                currentYear++;
            }

            DisplayDate();

        } while (true);
    }

    private void NewMonthTasks()
    {
        emailGenerator.NewMonthCalculations();
    }

    private void DisplayDate()
    {
        dateTxt.text = months[currentMonth] + " " + currentYear;
    }
}
