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

    public static InventoryGUI instance;
    [HideInInspector]
    public GameObject inventoryGUI;



    private void Awake()
    {
        instance = this;
        inventoryGUI = gameObject;
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
}
