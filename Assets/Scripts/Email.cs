using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Email : MonoBehaviour
{
    [Header("Info")]
    public string fromName;
    public string subjectLine;
    public string bodyTxt;
    public float orderAmt;
    public Transform spawnLocation;
    public GameObject buyerPrefab;
    public int timeTillCancel;
    public Text btnTxtFrom;
    public Text btnTxtSubject;

    private InboxController inboxController;
    private Button btn;

    private void Start()
    {
        if (!inboxController)
            inboxController = InboxController.instance.inboxController.GetComponent<InboxController>();

        if (!btn)
            btn = GetComponent<Button>();

        btn.onClick.AddListener(DisplayEmail);

        btnTxtFrom.text = "FROM: " + fromName;
        btnTxtSubject.text = "SUBJECT: " + subjectLine;
    }

    public void DisplayEmail()
    {
        inboxController.UpdateEmailTxt(fromName, subjectLine, bodyTxt, orderAmt, gameObject);
        inboxController.SetOrderInfoActive(true);
        //send info to confirm email button for buyer spawn and to set timeTIllCancel
    }



}
