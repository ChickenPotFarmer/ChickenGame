using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdminBar : MonoBehaviour
{
    public InputField txtInput;
    public CanvasGroup cg;

    [Header("Debug Items")]
    public GameObject seedBag1;
    public GameObject seedBag2;
    public GameObject blueDreamBrick;

    private InventoryController inventoryController;
    private Bank bank;

    private void Start()
    {
        if (!inventoryController)
            inventoryController = InventoryController.instance.inventoryController.GetComponent<InventoryController>();

        if (!bank)
            bank = Bank.instance.bank.GetComponent<Bank>();
    }

    private void Update()
    {
        if (Input.GetKeyDown("`"))
            SetPanelActive(true);
    }

    public void ParseCommand()
    {
        string[] words = txtInput.text.Split(' ');

        for (int i = 0; i < words.Length; i++)
        {
            words[i] = words[i].ToUpper();
        }

        if (words[0].Equals("SPAWN"))
        {
            if (words[1].Equals("SB1"))
            {
                int amt = int.Parse(words[2]);
                GameObject bag;
                for (int i = 0; i < amt; i++)
                {
                    bag = Instantiate(seedBag1);

                    inventoryController.ReturnToInventory(bag.GetComponent<InventoryItem>());
                }

            }
            else if (words[1].Equals("SB2"))
            {
                GameObject bag = Instantiate(seedBag2);

                inventoryController.ReturnToInventory(bag.GetComponent<InventoryItem>());

            }
        }
        else if (words[0].Equals("1"))
        {
            int amt = 2;
            GameObject bag;
            for (int i = 0; i < amt; i++)
            {
                bag = Instantiate(seedBag1);

                inventoryController.ReturnToInventory(bag.GetComponent<InventoryItem>());

                bag = Instantiate(seedBag2);

                inventoryController.ReturnToInventory(bag.GetComponent<InventoryItem>());
            }

            bag = Instantiate(blueDreamBrick);
            inventoryController.ReturnToInventory(bag.GetComponent<InventoryItem>());

        }

        else if (words[0].Equals("2"))
        {
            GameObject bag;

            bag = Instantiate(blueDreamBrick);
            inventoryController.ReturnToInventory(bag.GetComponent<InventoryItem>());

        }

        else if (words[0].Equals("ADDCASH"))
        {
            float amt = float.Parse(words[1]);
            bank.RequestCash(amt);
        }
        SetPanelActive(false);
    }

    public void SetPanelActive(bool _active)
    {
        if (_active)
        {
            cg.alpha = 1;
            cg.interactable = true;
            cg.blocksRaycasts = true;
            txtInput.ActivateInputField();
        }
        else
        {
            cg.alpha = 0;
            cg.interactable = false;
            cg.blocksRaycasts = false;
        }
    }

}
