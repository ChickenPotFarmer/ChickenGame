using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Buyer : MonoBehaviour
{
    [Header("Info")]
    public string buyerEmail;
    public float amountRequested;
    public float totalPay;
    public ToDoObject toDoObject;

    [Header("Hover Info")]
    public CanvasGroup hoverInfoCg;
    public Text deliverTxt;

    public InventoryController inventoryController;

    private void Start()
    {
        if (!inventoryController)
            inventoryController = InventoryController.instance.inventoryController.GetComponent<InventoryController>();
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
