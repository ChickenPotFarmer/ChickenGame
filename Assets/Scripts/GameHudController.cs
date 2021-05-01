﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameHudController : MonoBehaviour
{
    [Header("Setup")]
    public Text dryWeedTxt;
    public Text wetWeedTxt;
    public Text totalWeedTxt;
    public Text cashTxt;

    private InventoryController inventoryController;

    private void Start()
    {
        if (!inventoryController)
            inventoryController = InventoryController.instance.inventoryController.GetComponent<InventoryController>();

        StartCoroutine(UpdateUIRoutine());
    }

    IEnumerator UpdateUIRoutine()
    {
        do
        {
            dryWeedTxt.text = inventoryController.dryGramsCarrying.ToString("n1");
            wetWeedTxt.text = inventoryController.wetGramsCarrying.ToString("n1");
            totalWeedTxt.text = inventoryController.totalGramsCarrying.ToString("n1");
            cashTxt.text = "$" + inventoryController.moneyCarrying.ToString("n2");
            yield return new WaitForSeconds(0.2f);
        } while (true);
    }
}
