using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartDropdown : MonoBehaviour
{
    [Header("Status")]
    public bool storageCrateOpen;
    private StorageCrate openedCrate;

    [Header("Setup")]
    public GameObject dropDownBtn;
    public Transform parentDropDown;
    public CanvasGroup cg;

    private InventoryController inventoryController;
    private StrainInfoUI strainInfoUI;

    public static SmartDropdown instance;
    [HideInInspector]
    public GameObject smartDropdown;

    private void Awake()
    {
        instance = this;
        smartDropdown = gameObject;
    }

    private void Start()
    {
        if (!inventoryController)
            inventoryController = InventoryController.instance.inventoryController.GetComponent<InventoryController>();

        if (!strainInfoUI)
            strainInfoUI = StrainInfoUI.instance.strainInfoUI.GetComponent<StrainInfoUI>();
    }

    public void SetStorageDropdown(StorageCrate _storageContainer)
    {
        openedCrate = _storageContainer;
        storageCrateOpen = true;

    }

    public void UnsetStorage()
    {
        openedCrate = null;
        storageCrateOpen = false;
    }

    public void OpenDropdown(InventoryItem _itemClicked)
    {
        if (_itemClicked.inPlayerInventory)
        {
            //TransferAllToStorageSpawn(openedCrate);


        }
        else
        {
            TransferAllFromStorageSpawn(_itemClicked);
        }
        SetDropdownActive(true);
    }

    private void TransferAllToStorageSpawn()
    {
        
    }

    private void TransferAllFromStorageSpawn(InventoryItem _itemClicked)
    {
        GameObject newBtn = Instantiate(dropDownBtn, parentDropDown);
        DropDownBtn btnComp = newBtn.GetComponent<DropDownBtn>();

        btnComp.btnTxt.text = "Take All";

        btnComp.btnComp.onClick.AddListener(delegate { TransferFromStorageRoutine(_itemClicked); });
        print("spawned from storage btn");
    }

    public void TransferFromStorageRoutine(InventoryItem _itemClicked)
    {
        if (openedCrate)
        {
            inventoryController.ReturnAllItems(openedCrate.slotsParent, _itemClicked.itemID);
            CloseAndResetDropdown();
        }
    }

    public void StrainInfoSpawn(StrainProfile _strain)
    {
        GameObject newBtn = Instantiate(dropDownBtn, parentDropDown);
        DropDownBtn btnComp = newBtn.GetComponent<DropDownBtn>();

        btnComp.btnComp.onClick.AddListener(delegate { SetStrainInfoActive(_strain); });
    }

    public void SetStrainInfoActive(StrainProfile _strain)
    {
        strainInfoUI.SetStrainInfoActive(_strain);
        CloseAndResetDropdown();
    }

    public void CloseAndResetDropdown()
    {
        StartCoroutine(CloseAndResetDropdownRoutine());
    }

    public IEnumerator CloseAndResetDropdownRoutine()
    {
        Transform go;
        for (int i = 0; i < parentDropDown.childCount; i++)
        {
            go = parentDropDown.GetChild(i);
            go.SetParent(null, false);
            yield return new WaitForEndOfFrame();
            Destroy(go.gameObject);
        }

        SetDropdownActive(false);
    }


    public void SetDropdownActive(bool _active)
    {
        if (_active)
        {
            //active = true;
            transform.position = Input.mousePosition;
            cg.alpha = 1;
            cg.interactable = true;
            cg.blocksRaycasts = true;
        }
        else
        {
            //active = false;
            cg.alpha = 0;
            cg.interactable = false;
            cg.blocksRaycasts = false;
            
        }
    }



}
