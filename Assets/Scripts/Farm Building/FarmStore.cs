using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FarmStore : MonoBehaviour
{
    // These are buttons, not SeedBags
    [Header("Fields")]
    public GameObject[] commonSeeds;

    [Header("Setup")]
    public CanvasGroup storeCg;
    public CanvasGroup confirmPanelCg;
    public Button buyBtn;
    public Button cancelBtn;

    [Header("Confirm UI Info")]
    public TextMeshProUGUI objectNameTxt;

    public static FarmStore instance;
    [HideInInspector]
    public GameObject farmStore;

    private InventoryController inventoryController;
    private Transform openSlot;
    private FarmBuilderItem selectedItem;
    private Bank bank;

    private void Awake()
    {
        instance = this;
        farmStore = gameObject;
    }

    private void Start()
    {
        if (!inventoryController)
            inventoryController = InventoryController.instance.inventoryController.GetComponent<InventoryController>();

        if (!bank)
            bank = Bank.instance.bank.GetComponent<Bank>();

        buyBtn.onClick.AddListener(delegate { BuySelectedSeed(); });
        cancelBtn.onClick.AddListener(delegate { CancelBuy(); });
    }

    public void BuySelectedSeed()
    {
        // check if has open inventory slot
        //check if has enough cash

        if (GetOpenSlot() && inventoryController.CheckIfCanAfford(selectedItem.storeCost))
        {
            GameObject newItem = Instantiate(selectedItem.objectPrefab);
            InventoryItem itemComp = newItem.GetComponent<InventoryItem>();
            inventoryController.ReturnToInventory(itemComp);
            inventoryController.moneyCarrying -= selectedItem.storeCost;
            SetConfirmActive(false);
        }
        else
        {
            print("No Open Inventory Slots and/or Not Enough Money");
        }

    }

    public void SetSelectedItem(FarmBuilderItem _builderItem)
    {
        selectedItem = _builderItem;

        

        SetConfirmActive(true);
    }

    private bool GetOpenSlot()
    {
        bool slotFound;
        openSlot = inventoryController.GetOpenSlot();

        if (openSlot)
            slotFound = true;
        else
            slotFound = false;

        return slotFound;
    }

    public void CloseSeedStore()
    {
        SetStorePanelActive(false);
    }

    public void OpenSeedStore()
    {
        SetStorePanelActive(true);
    }


    // dont use confirm panel
    // confirm is implied when player places piece
    // add a button to undo last placed item?
    private void SetConfirmActive(bool _active)
    {
        if (_active)
        {
            confirmPanelCg.alpha = 1;
            confirmPanelCg.interactable = true;
            confirmPanelCg.blocksRaycasts = true;
        }
        else
        {
            confirmPanelCg.alpha = 0;
            confirmPanelCg.interactable = false;
            confirmPanelCg.blocksRaycasts = false;
        }
    }

    private void SetStorePanelActive(bool _active)
    {
        if (_active)
        {
            storeCg.alpha = 1;
            storeCg.interactable = true;
            storeCg.blocksRaycasts = true;
        }
        else
        {
            storeCg.alpha = 0;
            storeCg.interactable = false;
            storeCg.blocksRaycasts = false;
        }
    }

    public void CancelBuy()
    {
        selectedItem = null;
        SetConfirmActive(false);
    }
}
