using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Atm : MonoBehaviour
{
    [Header("Setup")]
    public Transform slotsParent;
    public Text bankAccountTxt;
    public CanvasGroup cg;
    public TMP_InputField amountInput;
    public GameObject interactIndicator;
    public string[] tagsAccepted;
    private bool playerInRange;
    private bool panelOpen;
    private List<Transform> slots = new List<Transform>();


    private Bank bank;
    private InventoryController inventoryController;
    private SmartDropdown smartDropdown;

    private void Start()
    {
        if (!inventoryController)
            inventoryController = InventoryController.instance.inventoryController.GetComponent<InventoryController>();

        if (!bank)
            bank = Bank.instance.bank.GetComponent<Bank>();

        if (!smartDropdown)
            smartDropdown = SmartDropdown.instance.smartDropdown.GetComponent<SmartDropdown>();

        UpdateUI();
        IntializeSlots();
    }

    private void Update()
    {
        if (playerInRange && !panelOpen)
        {
            if (Input.GetKeyDown("f"))
            {
                SetPanelActive(true);
            }
        }
    }

    private void UpdateUI()
    {
        bankAccountTxt.text = "$" + bank.bankAccount.ToString("n0");
    }

    private void IntializeSlots()
    {
        // Intialize Slots References
        if (slots.Count == 0)
        {
            for (int i = 0; i < slotsParent.childCount; i++)
            {
                slots.Add(slotsParent.GetChild(i));
            }
        }
    }

    public void DepositCash()
    {
        List<InventoryItem> slotItems = new List<InventoryItem>();
        float amtToDeposit = 0;

        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].childCount != 0)
                slotItems.Add(slots[i].GetChild(0).GetComponent<InventoryItem>());
        }

        for (int i = 0; i < slotItems.Count; i++)
        {
            amtToDeposit += slotItems[i].amount;
            Destroy(slotItems[i].gameObject);
        }

        bank.DepositCash(amtToDeposit);
        UpdateUI();
    }

    public void WithdrawCash()
    {
        float amt = float.Parse(amountInput.text);

        if (amt <= bank.bankAccount)
        {
            bank.WithdrawCash(float.Parse(amountInput.text));
            UpdateUI();
        }
        else
        {
            Alerts.DisplayMessage("Not enough money in bank account!");
        }
    }

    public void ClearInventory()
    {
        inventoryController.ReturnAllItems(slotsParent);
    }

    public void CloseATM()
    {
        SetPanelActive(false);
    }
    
    public void SetInteractable(bool _canInteract)
    {
        if (_canInteract)
        {
            if (!playerInRange)
            {
                playerInRange = true;
                interactIndicator.SetActive(true);
            }

        }
        else
        {
            if (playerInRange)
            {
                playerInRange = false;
                interactIndicator.SetActive(false);
            }
        }
    }

    public void SetPanelActive(bool _active)
    {
        if (_active)
        {
            cg.alpha = 1;
            cg.interactable = true;
            cg.blocksRaycasts = true;
            UpdateUI();
            smartDropdown.SetStorageDropdown(slotsParent, tagsAccepted);
            panelOpen = true;
        }
        else
        {
            cg.alpha = 0;
            cg.interactable = false;
            cg.blocksRaycasts = false;
            smartDropdown.UnsetStorage();
            panelOpen = false;
        }
    }
}
