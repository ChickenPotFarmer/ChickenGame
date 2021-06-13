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
            if (storageCrateOpen)
            {
                TransferAllOfTypeToStorageSpawn(_itemClicked.itemID);
                TransferAllToStorageSpawn();
            }

        }
        else
        {
            if (storageCrateOpen)
            {
                TransferAllOfTypeFromStorageSpawn(_itemClicked);
                TransferAllFromStorageSpawn();
            }
        }
        SetDropdownActive(true);
    }

    private void TransferAllToStorageSpawn()
    {
        GameObject newBtn = Instantiate(dropDownBtn, parentDropDown);
        DropDownBtn btnComp = newBtn.GetComponent<DropDownBtn>();

        btnComp.btnTxt.text = "Store All";

        btnComp.btnComp.onClick.AddListener(delegate { TransferAllToStorageRoutine(inventoryController.slotsParent); });

    }

    private void TransferAllOfTypeToStorageSpawn(string _itemId)
    {
        GameObject newBtn = Instantiate(dropDownBtn, parentDropDown);
        DropDownBtn btnComp = newBtn.GetComponent<DropDownBtn>();

        btnComp.btnTxt.text = "Store All of Type";

        btnComp.btnComp.onClick.AddListener(delegate { TransferAllToStorageRoutine(inventoryController.slotsParent, _itemId); });

    }

    private void TransferAllToStorageRoutine(Transform _slotsParent, string _itemId)
    {
        if (openedCrate)
        {
            openedCrate.ReturnAllItems(_slotsParent, _itemId);
            CloseAndResetDropdown();
        }
    }

    private void TransferAllToStorageRoutine(Transform _slotsParent)
    {
        if (openedCrate)
        {
            openedCrate.ReturnAllItems(_slotsParent);
            CloseAndResetDropdown();
        }
    }

    private void TransferAllFromStorageSpawn()
    {
        GameObject newBtn = Instantiate(dropDownBtn, parentDropDown);
        DropDownBtn btnComp = newBtn.GetComponent<DropDownBtn>();

        btnComp.btnTxt.text = "Take All";

        btnComp.btnComp.onClick.AddListener(delegate { TransferAllFromStorageRoutine(); });

    }

    private void TransferAllOfTypeFromStorageSpawn(InventoryItem _itemClicked)
    {
        GameObject newBtn = Instantiate(dropDownBtn, parentDropDown);
        DropDownBtn btnComp = newBtn.GetComponent<DropDownBtn>();

        btnComp.btnTxt.text = "Take All Of Type";

        btnComp.btnComp.onClick.AddListener(delegate { TransferAllOfTypeFromStorageRoutine(_itemClicked); });

    }


    public void TransferAllFromStorageRoutine()
    {
        if (openedCrate)
        {
            inventoryController.ReturnAllItems(openedCrate.slotsParent);
            CloseAndResetDropdown();
        }
    }

    public void TransferAllOfTypeFromStorageRoutine(InventoryItem _itemClicked)
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
        //StartCoroutine(CloseAndResetDropdownRoutine());
        ResetDropdownRoutine();
        SetDropdownActive(false);

    }

    public void ResetDropdownRoutine()
    {
        Transform go;
        int dropDownChilds = parentDropDown.childCount;
        for (int i = 0; i < dropDownChilds; i++)
        {
            go = parentDropDown.GetChild(0);
            go.SetParent(null, false);
            //yield return new WaitForEndOfFrame(); // to reduce the wait, break the SetParent and Destroy in to 2 serperate for statements seperated by the yield
            Destroy(go.gameObject);
        
        }


        //SetDropdownActive(false);

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
            ResetDropdownRoutine();

        }
    }



}
