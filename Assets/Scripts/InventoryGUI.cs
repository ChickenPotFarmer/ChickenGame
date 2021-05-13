using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryGUI : MonoBehaviour
{
    [Header("Animator")]
    public Animator inventoryAnimator;
    public bool isOpen;

    [Header("Setup")]
    public Transform openSlot;
    public Canvas inventoryCanvas;
    public Transform dragParent;
    public Transform slotsParent;
    private Transform[] inventorySlots;

    public static InventoryGUI instance;
    [HideInInspector]
    public GameObject inventoryGUI;

    private void Awake()
    {
        instance = this;
        inventoryGUI = gameObject;
    }

    private void Start()
    {
        inventorySlots = new Transform[slotsParent.childCount];
        for (int i = 0; i < slotsParent.childCount; i++)
        {
            inventorySlots[i] = slotsParent.GetChild(i);
        }
    }

    public void ToggleInventoryPanel()
    {
        if (isOpen)
        {
            inventoryAnimator.Play("Close");
            isOpen = false;
        }
        else
        {
            inventoryAnimator.Play("Open");
            isOpen = true;
        }
    }

    public Transform GetOpenSlot()
    { 
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (inventorySlots[i].childCount == 0)
            {
                openSlot = inventorySlots[i];
                break;
            }
        }

        return openSlot;

    }
}
