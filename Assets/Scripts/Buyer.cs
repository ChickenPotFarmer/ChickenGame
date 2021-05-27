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
    public float totalPay;
    public ToDoObject toDoObject;

    [Header("Buyer Prefabs")]
    public GameObject[] prefabs;
    public Transform modelSlot;

    [Header("Hover Info")]
    public CanvasGroup hoverInfoCg;
    public Text deliverTxt;

    [Header("Camera")]
    public GameObject cam;

    [Header("Setup")]
    public CanvasGroup cg;
    public Transform slotsParent;
    public Transform[] slots;
    public GameObject successParticles;

    private InventoryController inventoryController;
    private InventoryGUI inventoryGUI;

    private Animator buyerAnimator;
    public GameObject randomBuyer;

    private void Start()
    {
        if (!inventoryController)
            inventoryController = InventoryController.instance.inventoryController.GetComponent<InventoryController>();

        if (!inventoryGUI)
            inventoryGUI = InventoryGUI.instance.inventoryGUI.GetComponent<InventoryGUI>();

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

    private void Update()
    {
        if (Input.GetKeyDown("0"))
            WeirdClearInventory();
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

        Destroy(gameObject);
    }

    public void OpenBuyerPanel()
    {
        SetPanelActive(true);
        cam.SetActive(true);
        if (!inventoryGUI.isOpen)
            inventoryGUI.ToggleInventoryPanel();
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
            cg.alpha = 1;
            cg.interactable = true;
            cg.blocksRaycasts = true;
        }
        else
        {
            cg.alpha = 0;
            cg.interactable = false;
            cg.blocksRaycasts = false;
        }
    }

    // Add functions to check for correct strain type
    public float DropOffWeed(WeedBrick _weedBrick, float _amt)
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
            float remainder = inventoryController.ReturnToInventory(slotItems[i]);
            if (remainder == 0 || remainder == slotItems[i].amount)
            {
                slotItems.RemoveAt(i);

            }

        }
        amountInInventory = 0;
        orderFilled = false;
    }

    public void DeliverWeed()
    {
        if (orderFilled)
        {
            inventoryController.AddCash(totalPay);


            SaleSuccess();

            

        }
    }
}
