using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FarmStore : MonoBehaviour
{
    // These are buttons, not SeedBags
    [Header("Fields")]
    public GameObject[] placeables;

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
    private int selectedItem;
    private float selectedItemCost;
    private Bank bank;
    private FarmBuilder farmBuilder;

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

        if (!farmBuilder)
            farmBuilder = FarmBuilder.instance.farmBuilder.GetComponent<FarmBuilder>();
    }

    public bool BuySelected()
    {
        bool buyOk = false;

        if (bank.PayAmount(selectedItemCost))
        {
            buyOk = true;

            selectedItemCost = 0;
        }
        return buyOk;
    }

    public void SetSelectedItem(int _builderItem, float _cost)
    {
        selectedItem = _builderItem;
        selectedItemCost = _cost;

        farmBuilder.SelectPlaceable(placeables[selectedItem]);

        SetStorePanelActive(false);
    }

    public void CloseStore()
    {
        SetStorePanelActive(false);
    }

    public void OpenStore()
    {
        SetStorePanelActive(true);
    }

    public void UnselectItem()
    {
        selectedItem = 0;
        selectedItemCost = 0;
    }

    // dont use confirm panel
    // confirm is implied when player places piece
    // add a button to undo last placed item?
    //private void SetConfirmActive(bool _active)
    //{
    //    if (_active)
    //    {
    //        confirmPanelCg.alpha = 1;
    //        confirmPanelCg.interactable = true;
    //        confirmPanelCg.blocksRaycasts = true;
    //    }
    //    else
    //    {
    //        confirmPanelCg.alpha = 0;
    //        confirmPanelCg.interactable = false;
    //        confirmPanelCg.blocksRaycasts = false;
    //    }
    //}

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

    //public void CancelBuy()
    //{
    //    selectedItem = null;
    //    SetConfirmActive(false);
    //}
}
