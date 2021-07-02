using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmailGenerator : MonoBehaviour
{
    [Header("Status")]
    public int emailsPerMonth;
    public int minEmailsPerMonth;
    public int maxEmailsPerMonth;
    public int emailsPerMonthRemaining;
    public float minGramsPerOrder;
    public float maxGramsPerOrder;
    public float pricePerGram;

    private TimeLord timeLord;
    private float secsPerMonth;

    public static EmailGenerator instance;
    [HideInInspector]
    public GameObject emailGenerator;

    private void Awake()
    {
        instance = this;
        emailGenerator = gameObject;
    }

    private void Start()
    {
        if (!timeLord)
            timeLord = TimeLord.instance.timeLord.GetComponent<TimeLord>();

        secsPerMonth = timeLord.secsPerMonth;
    }

    public void NewMonthCalculations()
    {
        int playerLevel = Xp.GetPlayerLevel();

        maxGramsPerOrder = playerLevel * playerLevel * 4; // 4 is the constant that can be modified;
        minGramsPerOrder = maxGramsPerOrder * 0.35f;
        maxEmailsPerMonth = (int)(Mathf.Round((playerLevel / 5) + 0.49f) + 2);
        minEmailsPerMonth = maxEmailsPerMonth - 4;
        if (minEmailsPerMonth < 2)
            minEmailsPerMonth = 2;
        emailsPerMonthRemaining = emailsPerMonth;
        pricePerGram = 10f; // change this to "ReptuationManager"


        StartCoroutine(EmailGenerationRoutine());
    }

    IEnumerator EmailGenerationRoutine()
    {
        emailsPerMonth = Random.Range(minEmailsPerMonth, maxEmailsPerMonth);
        float timeBetweenEmails = secsPerMonth / emailsPerMonth;

        for (int i = 0; i < emailsPerMonth; i++)
        {
            GenerateEmail();
            yield return new WaitForSeconds(timeBetweenEmails);
        }
    }

    private void GenerateEmail()
    {
        print("email generated");
    }

}
