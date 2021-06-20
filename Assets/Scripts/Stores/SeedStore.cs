using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SeedStore : MonoBehaviour
{
    // These are buttons, not SeedBags
    [Header("Common Seeds")]
    public GameObject[] commonSeeds;

    [Header("Rare Seeds")]
    public GameObject[] rareSeeds;

    [Header("Legendary Seeds")]
    public GameObject[] legendarySeeds;

    [Header("Setup")]
    public CanvasGroup seedStoreCg;
    public CanvasGroup confirmPanelCg;
    public Button buyBtn;
    public Button cancelBtn;

    [Header("Confirm UI Info")]
    public TextMeshProUGUI strainNameTxt;
    public TextMeshProUGUI typeTxt;
    public TextMeshProUGUI thcTxt;
    public TextMeshProUGUI terpeneTxt;
    public TextMeshProUGUI primaryTerpeneTxt;
    public TextMeshProUGUI costTxt;


    public static SeedStore instance;
    [HideInInspector]
    public GameObject seedStore;

    private InventoryController inventoryController;
    private Transform openSlot;
    private StoreItem selectedSeed; 

    private void Awake()
    {
        instance = this;
        seedStore = gameObject;
    }

    private void Start()
    {
        inventoryController = InventoryController.instance.inventoryController.GetComponent<InventoryController>();

        buyBtn.onClick.AddListener(delegate { BuySelectedSeed(); });
        cancelBtn.onClick.AddListener(delegate { CancelBuy(); });
    }

    public void BuySelectedSeed()
    {
        // check if has open inventory slot
        //check if has enough cash

        if (GetOpenSlot() && inventoryController.CheckIfCanAfford(selectedSeed.storeCost))
        {
            GameObject newItem = Instantiate(selectedSeed.objectPrefab);
            InventoryItem itemComp = newItem.GetComponent<InventoryItem>();
            inventoryController.ReturnToInventory(itemComp);
            inventoryController.moneyCarrying -= selectedSeed.storeCost;
            SetConfirmActive(false);
        }
        else
        {
            print("No Open Inventory Slots and/or Not Enough Money");
        }

    }

    public void SetSelectedItem(StoreItem _storeItem)
    {
        selectedSeed = _storeItem;

        strainNameTxt.text = _storeItem.strainProfile.strainName;
        typeTxt.text = _storeItem.strainProfile.GetStrainType();
        thcTxt.text = _storeItem.strainProfile.GetReaderFriendlyThcContent();
        terpeneTxt.text = _storeItem.strainProfile.GetReaderFriendlyTerpeneContent();
        primaryTerpeneTxt.text = _storeItem.strainProfile.GetPrimaryTerpene();
        costTxt.text = "$" + _storeItem.storeCost.ToString("n2");

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
            seedStoreCg.alpha = 1;
            seedStoreCg.interactable = true;
            seedStoreCg.blocksRaycasts = true;
        }
        else
        {
            seedStoreCg.alpha = 0;
            seedStoreCg.interactable = false;
            seedStoreCg.blocksRaycasts = false;
        }
    }

    public void CancelBuy()
    {
        selectedSeed = null;
        SetConfirmActive(false);
    }

}
