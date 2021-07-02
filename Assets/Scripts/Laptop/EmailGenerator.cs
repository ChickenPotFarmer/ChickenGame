using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmailGenerator : MonoBehaviour
{
    [Header("Status")]
    public int emailsPerMonth;
    public int minEmailsPerMonth;
    public int maxEmailsPerMonth;
    public float minGramsPerOrder;
    public float maxGramsPerOrder;
    public float pricePerGram;

    [Header("Setup")]
    public GameObject emailPrefab;
    public Transform emailsParent;

    public TimeLord timeLord;
    private float secsPerMonth;
    public RandomEmail randomEmail;

    public static EmailGenerator instance;
    [HideInInspector]
    public GameObject emailGenerator;

    private void Awake()
    {
        instance = this;
        emailGenerator = gameObject;
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



        StartCoroutine(EmailGenerationRoutine());
    }

    IEnumerator EmailGenerationRoutine()
    {
        if (!timeLord)
            timeLord = TimeLord.instance.timeLord.GetComponent<TimeLord>();

        emailsPerMonth = Random.Range(minEmailsPerMonth, maxEmailsPerMonth);
        secsPerMonth = timeLord.secsPerMonth;
        float timeBetweenEmails = secsPerMonth / emailsPerMonth;

        for (int i = 0; i < emailsPerMonth; i++)
        {
            GenerateEmail();
            yield return new WaitForSeconds(timeBetweenEmails);
            print("time between emails: " + timeBetweenEmails);
        }
    }

    private void GenerateEmail()
    {
        GameObject newEmailObj = Instantiate(emailPrefab, emailsParent);
        Email newEmail = newEmailObj.GetComponent<Email>();

        newEmail.SetFromName(randomEmail.GenerateRandomEmailAddress());
        newEmail.SetOrderAmt(Random.Range(minGramsPerOrder, maxGramsPerOrder));
        newEmail.SetSubject("New Buy Order"); // generate in RandomEmail
        newEmail.SetBodyText("Test Body Text"); // generate in RandomEmail
        pricePerGram = 10f; // change this to "ReptuationManager"
        newEmail.SetPricePerGram(pricePerGram); // generate in RandomEmail

        newEmail.SetMinThc(.1f); // generate in RandomEmail
        newEmail.SetTypeRequested(-1); // generate in RandomEmail
        newEmail.SetTerpeneRequested(-1); // generate in RandomEmail
        newEmail.SetEffectRequested("NONE"); // generate in RandomEmail

        newEmail.SetTotalPay();

        Alerts.DisplayMessage("New e-mail from " + newEmail.fromName);
 
    }

}
