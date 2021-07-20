using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmailGenerator : MonoBehaviour
{
    [Header("Status")]
    public int emailsPerMonth;
    public int minEmailsPerMonth;
    public int maxEmailsPerMonth;
    public int activeOrders;
    public int maxActiveOrders;
    public float minGramsPerOrder;
    public float maxGramsPerOrder;

    [Header("Setup")]
    public GameObject emailPrefab;
    public Transform emailsParent;

    public TimeLord timeLord;
    private float secsPerMonth;
    public RandomEmail randomEmail;
    private ReputationManager reputationManager;
    private Coroutine generateRoutine;

    public static EmailGenerator instance;
    [HideInInspector]
    public GameObject emailGenerator;

    //private void Start()
    //{
    //    if (!reputationManager)
    //        reputationManager = ReputationManager.instance.repManager.GetComponent<ReputationManager>();
    //}

    private void Awake()
    {
        instance = this;
        emailGenerator = gameObject;
    }

    public void NewMonthCalculations()
    {
        int playerLevel = Xp.GetPlayerLevel();

        maxGramsPerOrder = playerLevel * playerLevel * 40; // 40 is the constant that can be modified;
        minGramsPerOrder = maxGramsPerOrder * 0.35f;
        maxEmailsPerMonth = (int)(Mathf.Round((playerLevel / 5) + 0.49f) + 2);

        if (maxActiveOrders != maxEmailsPerMonth)
            maxActiveOrders = maxEmailsPerMonth;

        minEmailsPerMonth = maxEmailsPerMonth - 4;
        if (minEmailsPerMonth < 2)
            minEmailsPerMonth = 2;

        if (generateRoutine != null)
            StopCoroutine(generateRoutine);
        generateRoutine = StartCoroutine(EmailGenerationRoutine());
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
            if (activeOrders < maxActiveOrders)
            {
                GenerateEmail();
                yield return new WaitForSeconds(timeBetweenEmails * timeLord.timeScale);
            }
            else
            {
                print("Not enough room in inbox, beginning check routine.");
                do
                {
                    if (activeOrders < maxActiveOrders)
                    {
                        GenerateEmail();
                    }
                    else
                    {
                        print("emails full, waiting 15 secs or until there is room.");
                        yield return new WaitForSeconds(15 * timeLord.timeScale);
                    }
                } while (activeOrders >= maxActiveOrders);
            }
        }
    }

    private void GenerateEmail()
    {
        if (!reputationManager)
            reputationManager = ReputationManager.instance.repManager.GetComponent<ReputationManager>();

        GameObject newEmailObj = Instantiate(emailPrefab, emailsParent);
        Email newEmail = newEmailObj.GetComponent<Email>();

        newEmail.SetFromName(randomEmail.GenerateRandomEmailAddress());

        float orderAmt = Random.Range(minGramsPerOrder, maxGramsPerOrder);

        //round to .X
        //orderAmt *= 10;
        orderAmt = Mathf.Round(orderAmt);
        //orderAmt /= 10;


        newEmail.SetOrderAmt(orderAmt);
        newEmail.SetSubject("New Buy Order"); // generate in RandomEmail
        newEmail.SetBodyText("Test Body Text"); // generate in RandomEmail

        newEmail.SetPricePerGram(reputationManager.GetPricePerGram());

        newEmail.SetMinThc(.1f); // generate in RandomEmail
        newEmail.SetTypeRequested(-1); // generate in RandomEmail
        newEmail.SetTerpeneRequested(-1); // generate in RandomEmail
        newEmail.SetEffectRequested("NONE"); // generate in RandomEmail

        newEmail.SetTotalPay();

        activeOrders++;

        Alerts.DisplayMessage("New e-mail from " + newEmail.fromName);
 
    }

}
