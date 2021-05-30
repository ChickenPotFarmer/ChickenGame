using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InboxController : MonoBehaviour
{
    [Header("Setup")]
    public CanvasGroup laptopPanelCg;
    public CanvasGroup orderInfoCg;
    public Transform emailsParent;
    public Text fromTxt;
    public Text subjectLineTxt;
    public Text emailBodyTxt;
    public Text amountRequestedTxt;
    public Text currentPerGramPriceTxt;
    public Text totalPayTxt;

    [Header("Current Email Info")]
    public Email currentEmail;
    public string fromName;
    public float amountRequested;
    public float totalPay;
    public GameObject currentInboxObject;

    [Header("Status")]
    public bool inboxEmpty;

    private WeedPriceMaster priceMaster;
    private BuyerController buyerController;
    private ToDoController toDoController;
    private LaptopController laptopController;

    public static InboxController instance;
    [HideInInspector]
    public GameObject inboxController;

    // Note for order spawning
    // terpene effects and requested terpenes MUST BE IN ORDER for checks to complete right

    private void Awake()
    {
        instance = this;
        inboxController = gameObject;
    }

    private void Start()
    {
        if (!priceMaster)
            priceMaster = WeedPriceMaster.instance.weedPriceMaster.GetComponent<WeedPriceMaster>();

        if (!buyerController)
            buyerController = BuyerController.instance.buyerContoller.GetComponent<BuyerController>();

        if (!toDoController)
            toDoController = ToDoController.instance.toDoController.GetComponent<ToDoController>();

        if (!laptopController)
            laptopController = LaptopController.instance.laptopController.GetComponent<LaptopController>();

        UpdateEmailTxt("", "", "", 0, null);

        StartCoroutine(InboxUpdateRoutine());
    }

    public void UpdateEmailTxt(string _from, string _subjectLine, string _bodyTxt, float _amount, GameObject _inboxBtn)
    {
        currentInboxObject = _inboxBtn;

        fromTxt.text = "FROM: " + _from;
        fromName = _from;
        subjectLineTxt.text = "SUBJECT: " + _subjectLine;
        emailBodyTxt.text = _bodyTxt;

        amountRequestedTxt.text = _amount.ToString("n1") + " G";
        amountRequested = _amount;

        currentPerGramPriceTxt.text = "$" + priceMaster.currentPrice.ToString("n2");

        totalPay = priceMaster.currentPrice * _amount;

        totalPayTxt.text = "$" + totalPay.ToString("n2");

    }

    IEnumerator InboxUpdateRoutine()
    {
        do
        {
            if (emailsParent.childCount == 0)
            {
                inboxEmpty = true;
                orderInfoCg.alpha = 0;
                orderInfoCg.interactable = false;
                orderInfoCg.blocksRaycasts = false;
                fromTxt.text = "";
                subjectLineTxt.text = "";
                emailBodyTxt.text = "";
            }
            //if (emailsParent.childCount > 0 && inboxEmpty)
            //{
            //    inboxEmpty = false;
            //    orderInfoCg.alpha = 1;
            //    orderInfoCg.interactable = true;
            //    orderInfoCg.blocksRaycasts = true;
            //    try
            //    {
            //        emailsParent.GetChild(0).GetComponent<Email>().DisplayEmail();
            //    }
            //    catch
            //    {
            //        Debug.LogWarning("Problem Displaying Email");
            //    }
            //}
            yield return new WaitForSeconds(1);
        } while (true);
    }

    public void TakeOrder()
    {
        buyerController.SpawnBuyer(currentEmail, toDoController.AddOrderToDo(amountRequested, fromName));
        Destroy(currentInboxObject);
        currentInboxObject = null;
        laptopController.clickActive = true;
        SetLaptopPanelActive(false);
    }

    public void SetLaptopPanelActive(bool _active)
    {
        if (_active)
        {
            laptopPanelCg.alpha = 1;
            laptopPanelCg.interactable = true;
            laptopPanelCg.blocksRaycasts = true;
        }
        else
        {
            laptopPanelCg.alpha = 0;
            laptopPanelCg.interactable = false;
            laptopPanelCg.blocksRaycasts = false;
            currentEmail = null;
        }
    }

    public void SetOrderInfoActive(bool _active)
    {
        if (_active)
        {
            orderInfoCg.alpha = 1;
            orderInfoCg.interactable = true;
            orderInfoCg.blocksRaycasts = true;
        }
        else
        {
            orderInfoCg.alpha = 0;
            orderInfoCg.interactable = false;
            orderInfoCg.blocksRaycasts = false;
            currentEmail = null;
        }
    }

}
