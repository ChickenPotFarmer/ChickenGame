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
    public bool orderFilled;
    public bool saleComplete;
    public float totalPay;
    public ToDoObject toDoObject;

    [Header("Order Constraints")]
    public int typeRequested; // -1 for any
    public int terpeneRequested;
    public float minThc;
    public string effectRequested;

    [Header("Buyer Prefabs")]
    public GameObject[] prefabs;
    public Transform modelSlot;

    [Header("Hover Info")]
    public CanvasGroup hoverInfoCg;
    public Text deliverTxt;

    [Header("Buyer UI")]
    public Text amountRequestedTxt;
    public Text typeRequestedTxt;
    public Text minThcTxt;
    public Text effectRequestedTxt;
    public Text terpeneRequestedTxt;

    [Header("Camera")]
    public GameObject cam;

    [Header("Setup")]
    public CanvasGroup cg;
    public CanvasGroup presaleBtnsCg;
    public CanvasGroup postsaleBtnsCg;

    public Transform slotsParent;
    public Transform[] slots;
    public GameObject successParticles;

    private InventoryController inventoryController;
    private InventoryGUI inventoryGUI;
    private Bank bank;
    private BuyerController buyerController;
    private SmartDropdown smartDropdown;

    private Animator buyerAnimator;
    public GameObject randomBuyer;

    // Note for order spawning
    // terpene effects and requested terpenes MUST BE IN ORDER for checks to complete right

    private void Start()
    {
        if (!inventoryController)
            inventoryController = InventoryController.instance.inventoryController.GetComponent<InventoryController>();

        if (!buyerController)
            buyerController = BuyerController.instance.buyerContoller.GetComponent<BuyerController>();

        if (!smartDropdown)
            smartDropdown = SmartDropdown.instance.smartDropdown.GetComponent<SmartDropdown>();

        //if (!inventoryGUI)
        //    inventoryGUI = InventoryGUI.instance.inventoryGUI.GetComponent<InventoryGUI>();

        if (!bank)
            bank = Bank.instance.bank.GetComponent<Bank>();

        if (randomBuyer == null)
        {
            randomBuyer = Instantiate(prefabs[Random.Range(0, prefabs.Length)], modelSlot);
            buyerAnimator = randomBuyer.GetComponent<Animator>();
        }


        slots = new Transform[slotsParent.childCount];
        // Intialize Slots References

        for (int i = 0; i < slotsParent.childCount; i++)
        {
            slots[i] = slotsParent.GetChild(i);
        }
        

    }

    public void SetInfo(Email _email, ToDoObject _toDo)
    {
        buyerEmail = _email.fromName;
        amountRequested = _email.orderAmt;
        totalPay = _email.totalPay;

        typeRequested = _email.typeRequested;
        terpeneRequested = _email.terpeneRequested;
        minThc = _email.minThc;
        effectRequested = _email.effectRequested;

        toDoObject = _toDo;
        deliverTxt.text = "Deliver " + amountRequested.ToString("n1") + " g";
        UpdateBuyerUI();
        SetHoverInfoActive(false);
    }

    public void UpdateBuyerUI()
    {
        amountRequestedTxt.text = amountRequested.ToString("n1") + " g";
        minThcTxt.text = (minThc * 100).ToString("n1") + "%";
        effectRequestedTxt.text = effectRequested;
        terpeneRequestedTxt.text = GetRequestedTerpene();
        typeRequestedTxt.text = GetTypeRequested();
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

    public void SaleSuccess()
    {
        StartCoroutine(SaleSuccessRoutine());
    }

    public IEnumerator SaleSuccessRoutine()
    {
        buyerAnimator.Play("Salute");
        SetPanelActive(false);

        yield return new WaitForSeconds(2);

        cam.SetActive(false);
        yield return new WaitForSeconds(2);

        // change this so they walk away or poof into a cloud
        GameObject particles = Instantiate(successParticles);
        particles.transform.position = transform.position;
        buyerController.BuyerOrderComplete();

        Xp.BuyComplete(amountRequested);

        Destroy(gameObject);
    }

    public void OpenBuyerPanel()
    {
        SetPanelActive(true);
        cam.SetActive(true);
        if (!inventoryController.inventoryActive)
            inventoryController.ToggleInventoryPanel();
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
            smartDropdown.SetBuyerDropdown(this);

            cg.alpha = 1;
            cg.interactable = true;
            cg.blocksRaycasts = true;
        }
        else
        {
            cg.alpha = 0;
            cg.interactable = false;
            cg.blocksRaycasts = false;
            smartDropdown.UnsetBuyer();
        }
    }

    public void SetPanelActive(CanvasGroup _cg, bool _active)
    {
        if (_active)
        {
            _cg.alpha = 1;
            _cg.interactable = true;
            _cg.blocksRaycasts = true;
        }
        else
        {
            _cg.alpha = 0;
            _cg.interactable = false;
            _cg.blocksRaycasts = false;
        }
    }

    // Add functions to check for correct strain type
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

        if (amountInInventory == amountRequested)
            orderFilled = true;

        return remainder;
    }

    public bool PassesInspection(StrainProfile _strain)
    {
        bool weedIsGood;

        weedIsGood = true;
        WeedBrick weedBrick = _strain.gameObject.GetComponent<WeedBrick>();

        if (weedBrick.isDry)
        {
            //Change these to grades, weight them and then add them to get a score

            // Strain Type Check
            if (Mathf.Abs(typeRequested - _strain.strainType) > 1 && typeRequested != -1)
            {
                Alerts.DisplayMessage("Weed is not the correct type! Buyer requested a " + _strain.GetStrainType(typeRequested) + " strain.");
                weedIsGood = false;
            }
            // Effect Check
            if (_strain.primaryEffect != effectRequested && effectRequested != "NONE")
            {
                Alerts.DisplayMessage("Weed rejected. This buyer requested the " + effectRequested + " effect!");
                weedIsGood = false;
            }
            // Terpene Check
            if (_strain.primaryTerpene != terpeneRequested && terpeneRequested != -1)
            {
                Alerts.DisplayMessage("Weed rejected. This buyer requested " + terpeneRequested + "!");
                weedIsGood = false;
            }
            // THC Check
            if (_strain.thcPercent < minThc)
            {
                Alerts.DisplayMessage("Weed rejected. This buyer requested at least " + (100 * minThc).ToString("n0") + "%!");
                weedIsGood = false;
            }
        }
        else
        {
            Alerts.DisplayMessage("Weed is not dry! Dry your weed in a dryer before trying to sell it.");
            weedIsGood = false;
        }

        if (weedIsGood)
            print("Weed Accepted");
        else
            print("Weed Rejected");

        return weedIsGood;
    }

    public void WeirdClearInventory()
    {
        List<InventoryItem> slotItems = new List<InventoryItem>();

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].childCount != 0)
                slotItems.Add(slots[i].GetChild(0).GetComponent<InventoryItem>());
        }

        for (int i = 0; i < slotItems.Count; i++)
        {
            ClearInventory();
        }
    }


    // for some reason this only clears one at a time.
    public void ClearInventory()
    {
        List<InventoryItem> slotItems = new List<InventoryItem>();

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].childCount != 0)
                slotItems.Add(slots[i].GetChild(0).GetComponent<InventoryItem>());
        }

        for (int i = 0; i < slotItems.Count; i++)
        {
            inventoryController.ReturnToInventory(slotItems[i]);
        }
    }

    public void DestroyInventory()
    {
        List<InventoryItem> slotItems = new List<InventoryItem>();

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].childCount != 0)
                slotItems.Add(slots[i].GetChild(0).GetComponent<InventoryItem>());
        }

        for (int i = 0; i < slotItems.Count; i++)
        {
            Destroy(slotItems[i].gameObject);
        }
    }



    public void DeliverWeed()
    {
        if (orderFilled)
        {

            DestroyInventory();

            bank.RequestCash(totalPay, slotsParent, true);
            SetPanelActive(presaleBtnsCg, false);
            SetPanelActive(postsaleBtnsCg, true);
            StartCoroutine(CheckForSaleComplete());
            

        }
    }

    IEnumerator CheckForSaleComplete()
    {
        do
        {
            saleComplete = true;

            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].childCount != 0)
                    saleComplete = false;

            }
            yield return new WaitForSeconds(0.1f);
        } while (!saleComplete);

        SaleSuccess();

    }

    public string GetTypeRequested()
    {
        string type;
        switch (typeRequested)
        {
            case 0:
                type = "Indica";
                break;

            case 1:
                type = "Indica Hybrid";
                break;

            case 2:
                type = "Hybrid";
                break;

            case 3:
                type = "Sativa Hybrid";
                break;

            case 4:
                type = "Sativa";
                break;

            case -1:
                type = "Any";
                break;

            default:
                type = "STRAIN TYPE ERROR";
                break;
        }

        return type;
    }

    public string GetRequestedTerpene()
    {
        string terpene;

        switch (terpeneRequested)
        {
            case 0:
                terpene = "Caryophyllene";
                break;

            case 1:
                terpene = "Limonene";
                break;

            case 2:
                terpene = "Linalool";
                break;

            case 3:
                terpene = "Myrcene";
                break;

            case 4:
                terpene = "Pinene";
                break;

            case 5:
                terpene = "Terpinolene";
                break;

            case -1:
                terpene = "Any";
                break;

            default:
                terpene = "PRIMARY TERPENE ERROR";
                break;
        }

        return terpene;
    }
}
