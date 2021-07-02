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
    public Transform spawnLocation;
    public GameObject buyerPrefab;
    public int timeTillCancel;
    public Text btnTxtFrom;
    public Text btnTxtSubject;

    [Header("Strain Request Info")]
    public float orderAmt;
    public float totalPay;
    public float pricePerGram;
    public int typeRequested; // -1 for any
    public int terpeneRequested; // -1 for any
    public float minThc;
    public string effectRequested;

    private InboxController inboxController;
    private Button btn;

    // Note for order spawning
    // terpene effects and requested terpenes MUST BE IN ORDER for checks to complete right

    private void Start()
    {
        if (!inboxController)
            inboxController = InboxController.instance.inboxController.GetComponent<InboxController>();

        if (!btn)
            btn = GetComponent<Button>();

        btn.onClick.AddListener(DisplayEmail);

        SetButtonText();
    }

    public void SetFromName(string _str)
    {
        fromName = _str;
    }

    public void SetSubject(string _str)
    {
        subjectLine = _str;
    }

    public void SetBodyText(string _str)
    {
        bodyTxt = _str;
    }

    public void SetOrderAmt(float _amt)
    {
        orderAmt = _amt;
    }

    public void SetPricePerGram(float _amt)
    {
        pricePerGram = _amt;   
    }

    public void SetTypeRequested(int _type)
    {
        typeRequested = _type;
    }

    public void SetTerpeneRequested(int _terpene)
    {
        terpeneRequested = _terpene;
    }

    public void SetEffectRequested(string _effect)
    {
        effectRequested = _effect;
    }

    public void SetMinThc(float _minThc)
    {
        minThc = _minThc;
    }

    public void SetTotalPay()
    {
        totalPay = orderAmt * pricePerGram;

    }

    public void SetButtonText()
    {
        btnTxtFrom.text = "FROM: " + fromName;
        btnTxtSubject.text = "SUBJECT: " + subjectLine;
    }

    public void DisplayEmail()
    {
        //inboxController.UpdateEmailTxt(fromName, subjectLine, bodyTxt, orderAmt, gameObject);
        inboxController.UpdateEmailTxt(this, gameObject);
        inboxController.currentEmail = this;
        inboxController.SetOrderInfoActive(true);
        //send info to confirm email button for buyer spawn and to set timeTIllCancel
    }



}
