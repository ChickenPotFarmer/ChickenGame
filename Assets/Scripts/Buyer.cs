using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Buyer : MonoBehaviour
{
    [Header("Info")]
    public string buyerEmail;
    public float amountRequested;
    public float amountInInventory;
    public float totalPay;
    public ToDoObject toDoObject;

    [Header("Buyer Prefabs")]
    public GameObject[] prefabs;
    public Transform modelSlot;

    [Header("Hover Info")]
    public CanvasGroup hoverInfoCg;
    public Text deliverTxt;

    [Header("Camera")]
    public GameObject cam;

    [Header("Setup")]
    public CanvasGroup cg;

    public InventoryController inventoryController;

    private Animator buyerAnimator;
    public GameObject randomBuyer;

    private void Start()
    {
        if (!inventoryController)
            inventoryController = InventoryController.instance.inventoryController.GetComponent<InventoryController>();

        if (randomBuyer == null)
        {
            randomBuyer = Instantiate(prefabs[Random.Range(0, prefabs.Length)], modelSlot);
            buyerAnimator = randomBuyer.GetComponent<Animator>();
        }
        
    }

    private void Update()
    {
        if (Input.GetKeyDown("0"))
            StartCoroutine(SaleSuccessRoutine());
    }

    public void SetInfo(string _buyerEmail, float _amtRequested, float _totalPay, ToDoObject _toDo)
    {
        buyerEmail = _buyerEmail;
        amountRequested = _amtRequested;
        totalPay = _totalPay;
        toDoObject = _toDo;
        deliverTxt.text = "Deliver " + _amtRequested.ToString("n1") + " g";
        SetHoverInfoActive(false);
    }

    public void SetHoverInfoActive(bool _active)
    {
        if (_active)
        {
            hoverInfoCg.alpha = 1;
            hoverInfoCg.interactable = true;
            hoverInfoCg.blocksRaycasts = true;
        }
        else
        {
            hoverInfoCg.alpha = 0;
            hoverInfoCg.interactable = false;
            hoverInfoCg.blocksRaycasts = false;
        }
    }

    public IEnumerator SaleSuccessRoutine()
    {
        buyerAnimator.Play("Salute");
        yield return new WaitForSeconds(2);
        SetPanelActive(false);
        cam.SetActive(false);
    }

    public void OpenBuyerPanel()
    {
        SetPanelActive(true);
        cam.SetActive(true);

    }

    public void CloseBuyerPanel()
    {
        SetPanelActive(false);
        cam.SetActive(false);

    }

    public void SetPanelActive(bool _active)
    {
        if (_active)
        {
            cg.alpha = 1;
            cg.interactable = true;
            cg.blocksRaycasts = true;
        }
        else
        {
            cg.alpha = 0;
            cg.interactable = false;
            cg.blocksRaycasts = false;
        }
    }

    public float DropOffWeed(float _amt)
    {
        float remainder = 0;
        float diff;

        if (amountInInventory + _amt <= amountRequested)
        {
            amountInInventory += _amt;
        }
        else
        {
            diff = amountRequested - amountInInventory;
            remainder = _amt - diff;
            amountInInventory = amountRequested;
        }

        return remainder;
    }

    public bool DeliverWeed()
    {
        bool amountCorrect = true;

        //if (inventoryController.dryGramsCarrying >= amountRequested)
        //{
        //    if(inventoryController.AddCash(totalPay))
        //    {
        //        // REMOVE BUYER
        //        inventoryController.dryGramsCarrying -= amountRequested;
        //        toDoObject.Complete();
        //        Destroy(gameObject);
        //    }
        //}
        //else
        //{
        //    amountCorrect = false;
        //}
        return amountCorrect;
    }
}
