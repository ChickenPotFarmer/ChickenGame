using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryGUI : MonoBehaviour
{
    [Header("Animator")]
    public Animator inventoryAnimator;
    public bool isOpen;

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
